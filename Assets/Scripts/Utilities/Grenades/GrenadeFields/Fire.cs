using System;
using System.Collections;
using System.Collections.Generic;
using Player.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities.Grenades.GrenadeFields
{
    [RequireComponent(typeof(Collider))]
    public class Fire : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private ParticleSystem _particles;

        [SerializeField] private int _damage;
        [SerializeField] private float _tick;

        private List<FireEffect> _targetsInFire;
        
        private void Start()
        {
            StartCoroutine(LifeRoutine());
        }

        private IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(_duration);
            Destroy(gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<FireEffect>(out FireEffect effect))
            {
                if (_targetsInFire.Exists(e => e.Equals(effect)))
                    return;
            }
            
            if (other.TryGetComponent<FieldOfView>(out FieldOfView fov))
            {
                FireEffect target = fov.AddComponent<FireEffect>();
                target.Initialize(_damage, _tick);
                _targetsInFire.Add(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<FireEffect>(out FireEffect effect))
            {
                _targetsInFire.Remove(effect);
                other.gameObject.RemoveComponent<BlindEffect>();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            
        }
#endif
    }
}