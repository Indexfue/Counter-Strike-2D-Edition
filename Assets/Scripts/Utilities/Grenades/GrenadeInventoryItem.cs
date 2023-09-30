using System;
using UnityEngine;

namespace Utilities.Grenades
{
    [Serializable]
    public struct GrenadeInventoryItem
    {
        [SerializeField] private int _currentCapacity;
        
        [SerializeField] private string _title;
        [SerializeField] private GrenadeType _grenadeType;
        
        public GameObject Prefab { get; }
        public int MaxCapacity { get; }
        public int CurrentCapacity
        {
            get => _currentCapacity;
            set => _currentCapacity = Mathf.Clamp(value, 0, MaxCapacity);
        }

        public GrenadeType GrenadeType
        {
            get => _grenadeType;
            set
            {
                if (value != Prefab.GetComponent<Grenade>().GrenadeType)
                    throw new ArgumentException("Prefab GrenadeType is not equals InventoryItem GrenadeType");
                
                _grenadeType = value;
            }
        }

        public void Use(Transform thrower, GrenadeFlightMode flightMode)
        {
            if (_currentCapacity == 0) return;

            var gameObject = GameObject.Instantiate(Prefab, thrower.position, thrower.rotation);
            gameObject.GetComponent<Grenade>().Use(flightMode, thrower);
            CurrentCapacity -= 1;
        }

        public GrenadeInventoryItem(GameObject prefab, GrenadeType grenadeType, int maxCapacity)
        {
            Prefab = prefab;
            _grenadeType = grenadeType;
            MaxCapacity = maxCapacity;
            _currentCapacity = 1;
            _title = Prefab.name;
        }
    }
}