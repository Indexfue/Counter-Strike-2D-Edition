using System;
using UnityEngine;
using Weapons.Recoil;
using Random = UnityEngine.Random;

namespace Weapons
{
    [Serializable]
    public sealed class WeaponBallistics
    {
        [SerializeField, Range(0,5f)] private float _spreadRate;
        [SerializeField] private float _spreadRadius;
        [SerializeField] private bool _useSpread;

        [SerializeField, Range(1f,2f)] private float _recoilForce;
        [SerializeField] private RecoilPattern _recoilPattern;
        [SerializeField] private bool _useRecoil;
        
        private void GetSpread(ref Vector3 direction) => direction += new Vector3(GetRandomPoint(), 0, GetRandomPoint());

        private float GetRandomPoint() => Random.Range(-_spreadRadius, _spreadRadius) * _spreadRate;

        private void GetRecoil(ref Vector3 direction, int continiousShotCount)
        {
            var recoilPattern = _recoilPattern.GetRecoilPattern();
            direction += Vector3.forward * recoilPattern[continiousShotCount % recoilPattern.Length] * _recoilForce;
        }

        public Vector3 GetBulletDirection(Vector3 direction, int continiousShotCount)
        {
            if (_useRecoil) 
                GetRecoil(ref direction, continiousShotCount);
            if (_useSpread)
                GetSpread(ref direction);
            
            return direction;
        }
    }
}