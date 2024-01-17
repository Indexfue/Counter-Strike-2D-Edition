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
        [SerializeField, Range(0,100f)] private float spreadMovementRate;
        [SerializeField, Range(0.1f, 1f)] private float spreadRadius;
        [SerializeField] private bool useSpread;

        [SerializeField, Range(1f,100f)] private float recoilForce;
        [SerializeField] private RecoilPattern recoilPattern;
        [SerializeField] private bool useRecoil;

        public float SpreadRate => spreadMovementRate;
        public float SpreadRadius => spreadRadius;
        public bool UseSpread => useSpread;

        private void GetSpread(ref Vector3 direction, GameObject playerInstance)
        {
            //TODO: Make spread normal
            if (playerInstance.TryGetComponent(out PlayerMovement playerMovement))
            {
                float currentSpreadMovementRate = 1 + playerMovement.CurrentUnitSpeed * spreadMovementRate;
                direction += new Vector3(GetRandomPoint(currentSpreadMovementRate), 0, GetRandomPoint(currentSpreadMovementRate));
            }
        }

        private float GetRandomPoint(float delta) => Random.Range(-spreadRadius, spreadRadius) * delta;

        private void GetRecoil(ref Vector3 direction, int continiousShotCount)
        {
            var recoilPattern = this.recoilPattern.GetRecoilPattern();
            direction += new Vector3(1, 0, 1) * recoilPattern[continiousShotCount % recoilPattern.Length] * recoilForce;
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