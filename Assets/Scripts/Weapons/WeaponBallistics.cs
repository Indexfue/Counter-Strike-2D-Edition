using System;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using Weapons.Recoil;
using Random = UnityEngine.Random;

namespace Weapons
{
    [Serializable]
    public sealed class WeaponBallistics
    {
        [SerializeField, Range(0,5f)] private float spreadRate;
        [SerializeField] private float spreadRadius;
        [SerializeField] private bool useSpread;

        [SerializeField, Range(1f,2f)] private float recoilForce;
        [SerializeField] private RecoilPattern recoilPattern;
        [SerializeField] private bool useRecoil;

        public float SpreadRate => spreadRate;
        public float SpreadRadius => spreadRadius;
        public bool UseSpread => useSpread;

        private void GetSpread(ref Vector3 direction, GameObject playerInstance)
        {
            if (playerInstance.TryGetComponent(out PlayerMovement playerMovement))
            {
                float movementSpreadRate = playerMovement.CurrentMovementSpeed * spreadRadius;
                Debug.Log($"{-spreadRadius - movementSpreadRate}, {spreadRadius + movementSpreadRate}");
                direction += new Vector3(GetRandomPoint(movementSpreadRate), 0, 0);
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