using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "Weapons", order = 51)]
    public class WeaponSettings : ScriptableObject
    {
        [Header("Capacity/Duration")]
        [SerializeField] private int maxCapacity;
        [SerializeField] private float reloadDuration;
        [SerializeField] private float coolingWeaponDuration;
        
        [Header("Weapon type")]
        [SerializeField] private WeaponCastType weaponCastType;
        [SerializeField] private WeaponType weaponType;

        [Header("Shooting")]
        [SerializeField] private int damage;
        [SerializeField, Range(0, Mathf.Infinity)] private float shootDistance;
        [SerializeField] private float shootDelay;
        [SerializeField] private float shotsPerSecond;
        [SerializeField] private bool canClampShooting;
        [SerializeField] private WeaponBallistics weaponBallistics;
        
        [Header("Prefab")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform shootPoint;
        //[SerializeField] private ItemViewData viewData;
        
        [Header("Layer Mask")]
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask ignoreMask;

        public int MaxCapacity => maxCapacity;
        public float ReloadDuration => reloadDuration;
        public float CoolingWeaponDuration => coolingWeaponDuration;

        public WeaponCastType WeaponCastType => weaponCastType;
        public WeaponType WeaponType => weaponType;

        public int Damage => damage;
        public float ShootDistance => shootDistance;
        public float ShootDelay => shootDelay;
        public float ShotsPerSecond => 1f / shotsPerSecond;
        public bool CanClampShooting => canClampShooting;
        public WeaponBallistics WeaponBallistics => weaponBallistics;

        public GameObject Prefab => prefab;
        public Transform ShootPoint => shootPoint;
        //public ItemViewData ViewData => viewData;
        
        public LayerMask TargetMask => targetMask;
        public LayerMask ObstacleMask => obstacleMask;
        public LayerMask IgnoreMask => ignoreMask;

        public void Initialize(GameObject Prefab, Transform ShootPoint)
        {
            prefab = Prefab;
            shootPoint = ShootPoint;
        }
    }
}