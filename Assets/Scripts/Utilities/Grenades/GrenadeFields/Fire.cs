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
        [SerializeField] private float duration;
        [SerializeField] private ParticleSystem particles;

        [SerializeField] private int damage;
        [SerializeField] private float tick;

        private readonly List<FireEffect> _targetsInFire = new List<FireEffect>();
        
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
            foreach (var target in _targetsInFire)
            {
                Destroy(target);
            }
            
            Destroy(gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<FireEffect>(out FireEffect effect))
            {
                if (_targetsInFire.Contains(effect))
                    return;
            }
            
            if (other.TryGetComponent<FieldOfView>(out FieldOfView fov))
            {
                FireEffect target = fov.AddComponent<FireEffect>();
                target.Initialize(damage, tick);
                _targetsInFire.Add(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<FireEffect>(out FireEffect effect))
            {
                _targetsInFire.Remove(effect);
                Destroy(effect);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (TryGetComponent<Collider>(out Collider collider))
            {
                Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
            }

            if (_targetsInFire.Count > 0)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
        }
#endif
    }
}