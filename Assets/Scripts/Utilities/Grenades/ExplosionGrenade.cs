using System;
using System.Collections;
using System.Collections.Generic;
using Player.Effects;
using UnityEngine;

namespace Utilities.Grenades
{
    public class ExplosionGrenade : Grenade
    {
        [SerializeField] private float _explodeRadius;
        [SerializeField] private int _damage;

        private List<GameObject> explodedObjects;

        protected override void Explode()
        {
            if (TryOverlap(out explodedObjects))
            {
                foreach (var obj in explodedObjects)
                {
                    obj.AddComponent<ExplosionEffect>().Initialize(_damage);
                }
            }
        }

        private bool TryOverlap(out List<GameObject> explodedObjects)
        {
            Collider[] overlappedObjects = new Collider[32];
            explodedObjects = new List<GameObject>();

            if (Physics.OverlapSphereNonAlloc(transform.position, _explodeRadius, overlappedObjects, _targetMask) > 0)
            {
                foreach (var obj in overlappedObjects)
                {
                    Vector3 directionToTarget = (obj.transform.position - transform.position).normalized;
                    
                    if (!Physics.Raycast(transform.position, directionToTarget, _obstacleMask))
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
            Gizmos.DrawSphere(transform.position, _explodeRadius);
        }
#endif
    }
}