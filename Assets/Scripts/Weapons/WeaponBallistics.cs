using System;
using Player;
using UnityEngine;
using Weapons.Recoil;
using Random = UnityEngine.Random;

namespace Weapons
{
    [Serializable]
    public sealed class WeaponBallistics
    {
        private const float _movementSpeedSpreadRate = 0.35f;
        
        [SerializeField, Range(0,5f)] private float spreadRate;
        [SerializeField] private float spreadRadius;
        [SerializeField] private bool useSpread;

        [SerializeField, Range(1f,2f)] private float recoilForce;
        [SerializeField] private RecoilPattern recoilPattern;
        [SerializeField] private bool useRecoil;

        private void GetSpread(ref Vector3 direction, GameObject playerInstance)
        {
            if (playerInstance.TryGetComponent(out PlayerMovement playerMovement))
            {
                float movementSpreadRate = playerMovement.CurrentMovementSpeed * _movementSpeedSpreadRate;
                Debug.Log($"{-spreadRadius - movementSpreadRate}, {spreadRadius + movementSpreadRate}");
                direction += new Vector3(GetRandomPoint(movementSpreadRate), 0, GetRandomPoint(movementSpreadRate));
            }
        }

        private float GetRandomPoint(float delta) => Random.Range(-spreadRadius - delta, spreadRadius + delta) * spreadRate;

        private void GetRecoil(ref Vector3 direction, int continiousShotCount)
        {
            var recoilPattern = this.recoilPattern.GetRecoilPattern();
            direction += Vector3.forward * recoilPattern[continiousShotCount % recoilPattern.Length] * recoilForce;
        }

        public Vector3 GetBulletDirection(Vector3 direction, int continiousShotCount, GameObject playerInstance)
        {
            if (useRecoil) 
                GetRecoil(ref direction, continiousShotCount);
            if (useSpread)
                GetSpread(ref direction, playerInstance);
            
            return direction;
        }
    }
}