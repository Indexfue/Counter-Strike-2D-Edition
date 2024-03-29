﻿using System.Collections;
using Interfaces;
using UnityEngine;

namespace Utilities.Grenades
{
    [RequireComponent(typeof(GrenadeFlightLogic))]
    public abstract class Grenade : MonoBehaviour, IInventoryItem
    {
        [SerializeField] protected GrenadeType grenadeType;
        [SerializeField] protected float explosionTime;
        [SerializeField] protected LayerMask targetMask;
        [SerializeField] protected LayerMask obstacleMask;

        [SerializeField] protected GrenadeFlightLogic flightLogic;

        [SerializeField] protected Texture icon;

        public GrenadeType GrenadeType => grenadeType;
        public Texture Icon => icon;

        private void Start()
        {
            StartCoroutine(ExplodeRoutine());
        }

        protected IEnumerator ExplodeRoutine()
        {
            yield return new WaitForSeconds(explosionTime);
            Explode();
            Destroy(gameObject);
        }

        protected abstract void Explode();

        public virtual void Use(GrenadeFlightMode flightMode, Transform thrower)
        {
            flightLogic.Fly(flightMode, thrower);
        }

        public void Select()
        {
            gameObject.SetActive(true);
        }

        public void Deselect()
        {
            gameObject.SetActive(false);
        }
    }
}