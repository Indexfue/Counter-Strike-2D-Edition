using System.Collections;
using System.Collections.Generic;
using Effects;
using Player.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities.Grenades
{
    public sealed class FlashGrenade : Grenade
    {
        [SerializeField] private float _baseFlashTime;
        [SerializeField] private float _flashRadius;

        public void Initialize()
        {
            StartCoroutine(ExplodeRoutine());
        }

        protected override void Explode()
        {
            HashSet<FlashedTarget> flashedTargetsList = GetTargetsInFlashRadius();
            FlashTargets(flashedTargetsList);
        }

        private HashSet<FlashedTarget> GetTargetsInFlashRadius()
        {
            Collider[] targetsInFlashRadius = new Collider[32];
            HashSet<FlashedTarget> flashedTargets = new HashSet<FlashedTarget>();

            if (Physics.OverlapSphereNonAlloc(transform.position, _flashRadius, targetsInFlashRadius, _targetMask) > 0)
            {
                foreach (var target in targetsInFlashRadius)
                {
                    if (target == null) break;

                    Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                    {
                        var flashRate = GetFlashRateByTargetRotation(target.gameObject.transform);
                        var flashTime = GetFlashTimeByTargetRotation(target.gameObject.transform);
                        flashedTargets.Add(new FlashedTarget(target.gameObject, flashTime));
                    }
                }
            }

            return flashedTargets;
        }

        private void FlashTargets(HashSet<FlashedTarget> flashedTargets)
        {
            if (flashedTargets.Count == 0) return;
            
            foreach (FlashedTarget target in flashedTargets)
            {
                if (target.Target.TryGetComponent<FieldOfView>(out FieldOfView fov))
                {
                    fov.AddComponent<FlashEffect>().Initialize(target.FlashTime);
                }
            }
        }

        private float GetFlashTimeByTargetRotation(Transform target)
        {
            return _baseFlashTime * GetFlashRateByTargetRotation(target);
        }

        private float GetFlashRateByTargetRotation(Transform target)
        {
            return 1 - (GetAngle(target) / 180f);
        }

        private float GetAngle(Transform target)
        {
            Ray ray = new Ray(transform.position, target.position - transform.position);

            float targetAngle = Vector3.Angle(target.TransformDirection(Vector3.forward), ray.direction);
            targetAngle = Mathf.Abs(targetAngle - 180);
            return targetAngle;
        }
    }

    public struct FlashedTarget
    {
        public GameObject Target { get; }
        public float FlashTime { get; }

        public FlashedTarget(GameObject target, float flashTime)
        {
            Target = target;
            FlashTime = flashTime;
        }
    }
}