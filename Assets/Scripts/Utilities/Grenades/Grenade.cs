using System;
using System.Collections;
using UnityEngine;

namespace Utilities.Grenades
{
    [RequireComponent(typeof(GrenadeFlightLogic))]
    public abstract class Grenade : Utility
    {
        [SerializeField] protected GrenadeType _grenadeType;
        [SerializeField] protected float _explosionTime;
        [SerializeField] protected LayerMask _targetMask;
        [SerializeField] protected LayerMask _obstacleMask;
        
        [SerializeField] protected GrenadeFlightLogic _flightLogic;

        protected abstract IEnumerator Explode();
        public abstract void Use(GrenadeFlightMode flightMode, Transform thrower);
    }
}