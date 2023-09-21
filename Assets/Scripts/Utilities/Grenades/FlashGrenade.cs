using System.Collections;
using System.Collections.Generic;
using Player.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities.Grenades
{
    public sealed class FlashGrenade : Grenade
    {
        [SerializeField] private float _baseFlashTime;
        [SerializeField] private float _flashRadius;

        public override void Use(GrenadeFlightMode flightMode, Transform thrower)
        {
            _flightLogic.Fly(flightMode, thrower);
        }

        protected override IEnumerator Explode()
        {
            yield return new WaitForSeconds(_explosionTime);
            List<FlashedTarget> flashedTargetsList = GetTargetsInFlashRadius();
            FlashTargets(flashedTargetsList);
            Destroy(gameObject);
        }
        
        private void Start()
        {
            StartCoroutine(Explode());
        }

        private List<FlashedTarget> GetTargetsInFlashRadius()
        {
            Collider[] targetsInFlashRadius = new Collider[32];
            List<FlashedTarget> flashedTargets = new List<FlashedTarget>();

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
                        flashedTargets.Add(new FlashedTarget(target.gameObject, flashTime, flashRate));
                    }
                }
            }

            return flashedTargets;
        }

        private void FlashTargets(List<FlashedTarget> flashedTargets)
        {
            if (flashedTargets.Count == 0) return;
            
            foreach (FlashedTarget target in flashedTargets)
            {
                if (target.Target.TryGetComponent<FieldOfView>(out FieldOfView fov))
                {
                    fov.AddComponent<BlindEffect>().Initialize(target.FlashRate, target.FlashTime);
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
        public float FlashRate { get; }

        public FlashedTarget(GameObject target, float flashTime, float flashRate)
        {
            Target = target;
            FlashTime = flashTime;
            FlashRate = flashRate;
        }
    }
}