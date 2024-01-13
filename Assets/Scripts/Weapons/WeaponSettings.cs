using Player;
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
        [SerializeField] private InventoryItemType inventoryItemType;

        [Header("Shooting")]
        [SerializeField] private int damage;
        [SerializeField, Range(0, Mathf.Infinity)] private float shootDistance;
        [SerializeField] private float shootDelay;
        [SerializeField] private float shotsPerSecond;
        [SerializeField] private bool canClampShooting;
        [SerializeField] private WeaponBallistics weaponBallistics;
        
        //[SerializeField] private ItemViewData viewData;
        
        [Header("Layer Mask")]
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask ignoreMask;

        public int MaxCapacity => maxCapacity;
        public float ReloadDuration => reloadDuration;
        public float CoolingWeaponDuration => coolingWeaponDuration;

        public WeaponCastType WeaponCastType => weaponCastType;
        public InventoryItemType InventoryItemType => inventoryItemType;

        public int Damage => damage;
        public float ShootDistance => shootDistance;
        public float ShootDelay => shootDelay;
        public float ShotsPerSecond => 1f / shotsPerSecond;
        public bool CanClampShooting => canClampShooting;
        public WeaponBallistics WeaponBallistics => weaponBallistics;
        
        //public ItemViewData ViewData => viewData;
        
        public LayerMask TargetMask => targetMask;
        public LayerMask ObstacleMask => obstacleMask;
        public LayerMask IgnoreMask => ignoreMask;
    }
}