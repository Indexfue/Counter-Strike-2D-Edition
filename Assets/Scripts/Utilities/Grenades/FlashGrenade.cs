using System.Collections;
using System.Collections.Generic;
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
            //Destroy(gameObject);
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
                        var flashTime = GetFlashTimeByTargetDistance(target.gameObject.transform);
                        flashedTargets.Add(new FlashedTarget(target.gameObject, flashRate, flashTime));
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
                    //TODO: AddComponent<BlindEffect> : Effect
                    fov.SetBlindEffect(target.FlashTime, target.FlashRate);
                }
            }
        }

        private float GetFlashTimeByTargetDistance(Transform target)
        {
            float targetDistance = Vector3.Distance(target.position, transform.position);
            return _baseFlashTime;
        }

        private float GetFlashRateByTargetRotation(Transform target)
        {
            Ray ray = new Ray(transform.position, target.position);
            if (!Physics.Raycast(ray, 50f, _obstacleMask))
            {
                float targetAngle = Vector3.Angle(target.TransformDirection(target.forward), ray.direction);

                targetAngle = Mathf.Abs(targetAngle - 180);
                return 1 - (targetAngle / 180f);
            }
            return 0;
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