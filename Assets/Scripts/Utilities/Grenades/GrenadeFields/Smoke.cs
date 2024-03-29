﻿using System.Collections;
using System.Collections.Generic;
using Player.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities.Grenades.GrenadeFields
{
    [RequireComponent(typeof(Collider))]
    public class Smoke : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private ParticleSystem particles;

        [SerializeField] private float blindRate;

        private readonly HashSet<Collider> _targetsInSmoke = new HashSet<Collider>();

        private void Start()
        {
            StartCoroutine(LifeRoutine());
        }

        private IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(duration);
            Die();
        }

        private void Die()
        {
            foreach (var target in _targetsInSmoke)
            {
                target.gameObject.RemoveComponent<BlindEffect>();
            }
            
            Destroy(gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<FieldOfView>(out FieldOfView fov))
            {
                if (!_targetsInSmoke.Contains(other))
                {
                    fov.AddComponent<BlindEffect>().Initialize();
                    _targetsInSmoke.Add(other);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_targetsInSmoke.Contains(other))
            {
                _targetsInSmoke.Remove(other);
                other.gameObject.RemoveComponent<BlindEffect>();
            }
        }
    }
}