using System;
using Player;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "Weapons", order = 51)]
    public class WeaponSettings : ScriptableObject
    {
        [Header("Capacity/Duration")]
        [SerializeField] private int _maxCapacity;
        [SerializeField] private float _reloadDuration;
        [SerializeField] private float _coolingWeaponDuration;
        
        [Header("Weapon type")]
        [SerializeField] private WeaponCastType _weaponCastType;
        [SerializeField] private WeaponType _weaponType;

        [Header("Shooting")]
        [SerializeField] private int _damage;
        [SerializeField, Range(0, Mathf.Infinity)] private float _shootDistance;
        [SerializeField] private float _shootDelay;
        [SerializeField] private Ticker _ticker;
        [SerializeField] private bool _canClampShooting;
        [SerializeField] private WeaponBallistics _weaponBallistics;
        
        [Header("Prefab")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _shootPoint;
        
        [Header("Layer Mask")]
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _obstacleMask;

        public int MaxCapacity => _maxCapacity;
        public float ReloadDuration => _reloadDuration;
        public float CoolingWeaponDuration => _coolingWeaponDuration;

        public WeaponCastType WeaponCastType => _weaponCastType;
        public WeaponType WeaponType => _weaponType;

        public int Damage => _damage;
        public float ShootDistance => _shootDistance;
        public float ShootDelay => _shootDelay;
        public Ticker Ticker => _ticker;
        public bool CanClampShooting => _canClampShooting;
        public WeaponBallistics WeaponBallistics => _weaponBallistics;

        public GameObject Prefab => _prefab;
        public Transform ShootPoint => _shootPoint;

        public LayerMask TargetMask => _targetMask;
        public LayerMask ObstacleMask => _obstacleMask;

        public void Initialize(GameObject prefab, Transform shootPoint)
        {
            _prefab = prefab;
            _shootPoint = shootPoint;
        }
    }
}