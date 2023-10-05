using System.Collections;
using Player.UI.Weapons;
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

        [SerializeField] protected Texture _icon;

        public GrenadeType GrenadeType => _grenadeType;
        public Texture Icon => _icon;

        private void Start()
        {
            StartCoroutine(ExplodeRoutine());
        }

        protected IEnumerator ExplodeRoutine()
        {
            yield return new WaitForSeconds(_explosionTime);
            Explode();
            Destroy(gameObject);
        }

        protected abstract void Explode();

        public virtual void Use(GrenadeFlightMode flightMode, Transform thrower)
        {
            _flightLogic.Fly(flightMode, thrower);
        }
    }
}