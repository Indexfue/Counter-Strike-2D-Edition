using System;
using Player;
using Player.UI.Weapons;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utilities.Grenades
{
    [Serializable]
    public struct GrenadeInventoryItem
    {
        [SerializeField] private int currentCapacity;
        
        [SerializeField] private string title;
        [SerializeField] private GrenadeType grenadeType;
        
        [SerializeField] private ItemViewData viewData;

        public GameObject Prefab { get; }
        public int MaxCapacity { get; }
        public int CurrentCapacity
        {
            get => currentCapacity;
            set => currentCapacity = Mathf.Clamp(value, 0, MaxCapacity);
        }

        public GrenadeType GrenadeType
        {
            get => grenadeType;
            set
            {
                if (value != Prefab.GetComponent<Grenade>().GrenadeType)
                    throw new ArgumentException("Prefab GrenadeType is not equals InventoryItem GrenadeType");
                
                grenadeType = value;
            }
        }
        
        public ItemViewData ViewData => viewData;

        public void Use(Transform thrower, GrenadeFlightMode flightMode)
        {
            if (currentCapacity == 0) return;

            var gameObject = GameObject.Instantiate(Prefab, thrower.position, thrower.rotation);
            gameObject.GetComponent<Grenade>().Use(flightMode, thrower);
            CurrentCapacity -= 1;
        }

        public GrenadeInventoryItem(GameObject prefab, GrenadeType grenadeType, int maxCapacity)
        {
            this.grenadeType = grenadeType;
            MaxCapacity = maxCapacity;
            currentCapacity = 1;
            Prefab = prefab;
            title = Prefab.name;
            
            Grenade grenade = prefab.GetComponent<Grenade>();
            viewData = new ItemViewData(title, grenade.Icon, InventoryItemType.Grenade);
        }
    }
}