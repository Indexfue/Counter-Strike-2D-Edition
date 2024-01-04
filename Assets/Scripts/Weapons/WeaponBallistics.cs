using System;
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
        
        private void GetSpread(ref Vector3 direction) => direction += new Vector3(GetRandomPoint(), 0, GetRandomPoint());

        private float GetRandomPoint() => Random.Range(-spreadRadius, spreadRadius) * spreadRate;

        private void GetRecoil(ref Vector3 direction, int continiousShotCount)
        {
            var recoilPattern = this.recoilPattern.GetRecoilPattern();
            direction += Vector3.forward * recoilPattern[continiousShotCount % recoilPattern.Length] * recoilForce;
        }

        public Vector3 GetBulletDirection(Vector3 direction, int continiousShotCount)
        {
            if (useRecoil) 
                GetRecoil(ref direction, continiousShotCount);
            if (useSpread)
                GetSpread(ref direction);
            
            return direction;
        }
    }
}