using System.Collections.Generic;
using Player.Effects;
using UnityEngine;

namespace Utilities.Grenades
{
    public class ExplosionGrenade : Grenade
    {
        [SerializeField] private float explodeRadius;
        [SerializeField] private int damage;

        private List<GameObject> _explodedObjects;

        protected override void Explode()
        {
            if (TryOverlap(out _explodedObjects))
            {
                foreach (var obj in _explodedObjects)
                {
                    obj.AddComponent<ExplosionEffect>().Initialize(damage);
                }
            }
        }

        private bool TryOverlap(out List<GameObject> explodedObjects)
        {
            Collider[] overlappedObjects = new Collider[32];
            explodedObjects = new List<GameObject>();

            if (Physics.OverlapSphereNonAlloc(transform.position, explodeRadius, overlappedObjects, targetMask) > 0)
            {
                foreach (var obj in overlappedObjects)
                {
                    if (obj == null) break;
                    
                    Vector3 directionToTarget = (obj.transform.position - transform.position).normalized;
                    
                    if (!Physics.Raycast(transform.position, directionToTarget, obstacleMask))
                    {
                        explodedObjects.Add(obj.gameObject);
                    }
                }
            }

            if (explodedObjects.Count == 0)
            {
                return false;
            }
            return true;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, explodeRadius);

            if (Physics.OverlapSphere(transform.position, explodeRadius, targetMask).Length > 0)
            {
                Gizmos.color = Color.red;
            }
        }
#endif
    }
}