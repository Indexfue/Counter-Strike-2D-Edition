using System;
using UnityEngine;

namespace Utilities.Grenades
{
    [Serializable]
    public struct GrenadeInventoryItem
    {
        [SerializeField] private int _currentCapacity;
        
        [SerializeField] private string _title;
        
        public GameObject Prefab { get; }
        public int MaxCapacity { get; }
        public int CurrentCapacity
        {
            get => _currentCapacity;
            set => _currentCapacity = Mathf.Clamp(value, 0, MaxCapacity);
        }

        public void Use(Transform thrower, GrenadeFlightMode flightMode)
        {
            if (_currentCapacity == 0) return;

            var gameObject = GameObject.Instantiate(Prefab, thrower.position, thrower.rotation);
            gameObject.GetComponent<Grenade>().Use(flightMode, thrower);
            CurrentCapacity -= 1;
        }

        public GrenadeInventoryItem(GameObject prefab, int maxCapacity)
        {
            Prefab = prefab;
            MaxCapacity = maxCapacity;
            _currentCapacity = 0;
            _title = Prefab.name;
        }
    }
}