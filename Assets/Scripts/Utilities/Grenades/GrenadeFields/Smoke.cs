using System;
using System.Collections;
using System.Collections.Generic;
using Player.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities.Grenades.GrenadeFields
{
    [RequireComponent(typeof(Collider))]
    public class Smoke : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private ParticleSystem _particles;

        [SerializeField] private float _blindRate;
        
        private List<BlindEffect> _targetsInSmoke;

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
            if (other.TryGetComponent<BlindEffect>(out BlindEffect effect))
            {
                if (_targetsInSmoke.Exists(e => e.Equals(effect)))
                    return;
            }
            
            if (other.TryGetComponent<FieldOfView>(out FieldOfView fov))
            {
                BlindEffect target = fov.AddComponent<BlindEffect>();
                target.Initialize(_blindRate, 0f, true);
                _targetsInSmoke.Add(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<BlindEffect>(out BlindEffect effect))
            {
                _targetsInSmoke.Remove(effect);
                other.gameObject.RemoveComponent<BlindEffect>();
            }
        }
    }
}