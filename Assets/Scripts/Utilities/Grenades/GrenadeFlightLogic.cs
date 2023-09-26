using System;
using System.Collections;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities.Grenades
{
    [Serializable]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public sealed class GrenadeFlightLogic : MonoBehaviour
    {
        [SerializeField] private float _baseFlySpeed;
        [SerializeField, Range(0, 1f)] private float _ricochetFlySpeedReduceRate;

        private float _currentFlySpeed;
        private Rigidbody _rigidbody;

        public void Fly(GrenadeFlightMode flightMode, Transform thrower)
        {
            SetFlySpeed(flightMode);
            StartCoroutine(SetSpeedByTime());
            AddForce(thrower.TransformDirection(Vector3.forward) * _currentFlySpeed, ForceMode.Impulse);
        }

        private void AddForce(Vector3 direction, ForceMode forceMode) => _rigidbody.AddForce(direction, forceMode);

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!TryGetComponent<HealthComponent>(out HealthComponent health))
            {
                Vector3 direction = GetNextFlyTrajectory(other.collider.gameObject.layer);
                SetSpeedByObstacleRicochet();
                AddForce(direction * _currentFlySpeed, ForceMode.Impulse);
            }
        }

        private void SetSpeedByObstacleRicochet() => _currentFlySpeed *= (1 - _ricochetFlySpeedReduceRate);

        private IEnumerator SetSpeedByTime()
        {
            while (_currentFlySpeed != 0)
            {
                _currentFlySpeed -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void SetFlySpeed(GrenadeFlightMode flightMode) => _currentFlySpeed = _baseFlySpeed * ((float)flightMode / 10f);

        private Vector3 GetNextFlyTrajectory(LayerMask obstacleLayer)
        {
            RaycastHit hit;
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100f, obstacleLayer))
            {
                return Vector3.Reflect(direction, hit.normal);
            }

            return direction;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            
        }

        private Vector3 GetExplosionPosition()
        {
            return new Vector3();
        }
#endif
    }
}