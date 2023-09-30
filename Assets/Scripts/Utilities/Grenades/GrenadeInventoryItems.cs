using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities.Grenades
{
    public class GrenadeInventoryItems : MonoBehaviour, IInventoryItem
    {
        [SerializeField] private Transform _throwPosition;
        [SerializeField] private SerializableDictionary<GrenadeType, GrenadeInventoryItem> _grenades;
        [SerializeField] private GrenadeInventoryItem _currentSelectedGrenade;

        private bool _leftClicked;
        private bool _rightClicked;

        public GrenadeInventoryItem CurrentSelectedGrenade => _currentSelectedGrenade;

        private void Start()
        {
            _grenades = new SerializableDictionary<GrenadeType, GrenadeInventoryItem>()
            {
                [GrenadeType.Flash] = new(Resources.Load("Utilities/Grenades/Flashbang Grenade").GameObject(), GrenadeType.Flash, 2),
                [GrenadeType.Explosion] = new(Resources.Load("Utilities/Grenades/Explosion Grenade").GameObject(), GrenadeType.Explosion, 1),
                [GrenadeType.Smoke] = new(Resources.Load("Utilities/Grenades/Smoke Grenade").GameObject(), GrenadeType.Smoke, 1),
                [GrenadeType.Fire] = new(Resources.Load("Utilities/Grenades/Fire Grenade").GameObject(), GrenadeType.Fire, 1)
            };

            _currentSelectedGrenade = _grenades[GrenadeType.Flash];
            SwitchGrenade();
        }

        private void OnEnable()
        {
            EventManager.Subscribe<FireKeyPressedEventArgs>(OnLeftButtonMousePerformed);
            EventManager.Subscribe<SecondaryFireKeyPressedEventArgs>(OnRightButtonMousePerformed);
            EventManager.Subscribe<FireKeyUnpressedEventArgs>(OnButtonMouseCanceled);
            EventManager.Subscribe<SecondaryFireKeyUnpressedEventArgs>(OnButtonMouseCanceled);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<FireKeyPressedEventArgs>(OnLeftButtonMousePerformed);
            EventManager.Unsubscribe<SecondaryFireKeyPressedEventArgs>(OnRightButtonMousePerformed);
            EventManager.Unsubscribe<FireKeyUnpressedEventArgs>(OnButtonMouseCanceled);
            EventManager.Unsubscribe<SecondaryFireKeyUnpressedEventArgs>(OnButtonMouseCanceled);
        }

        private void OnLeftButtonMousePerformed(FireKeyPressedEventArgs args) => _leftClicked = true;

        private void OnRightButtonMousePerformed(SecondaryFireKeyPressedEventArgs args) => _rightClicked = true;

        private void OnButtonMouseCanceled(EventArgs args)
        {
            if (!gameObject.activeSelf)
                return;
            
            if (_leftClicked)
                UseSelectedGrenade(GrenadeFlightMode.Hard);
            else if (_rightClicked)
                UseSelectedGrenade(GrenadeFlightMode.Light);

            _leftClicked = false;
            _rightClicked = false;
        }

        private void UseSelectedGrenade(GrenadeFlightMode flightMode)
        {
            _currentSelectedGrenade.Use(_throwPosition, flightMode);
        }

        private void SwitchGrenade()
        {
            _grenades[_currentSelectedGrenade.GrenadeType] = _currentSelectedGrenade;
            _currentSelectedGrenade = SetNextGrenade(_currentSelectedGrenade);
        }

        private GrenadeInventoryItem SetNextGrenade(GrenadeInventoryItem currentSelectedGrenade)
        {
            //TODO: CurrentCapacity = 0
            int currentGrenadeIndex = GetCurrentGrenadeIndex();

            for (int i = currentGrenadeIndex + 1; i <= currentGrenadeIndex + _grenades.Keys.Count; i++)
            {
                GrenadeInventoryItem nextGrenade = _grenades.Values.ElementAt(i % _grenades.Keys.Count);

                if (currentSelectedGrenade.GrenadeType == nextGrenade.GrenadeType || nextGrenade.CurrentCapacity == 0)
                {
                    continue;
                }

                return nextGrenade;
            }

            throw new Exception("Can't find item");
        }

        private int GetCurrentGrenadeIndex()
        {
            for (int i = 0; i < _grenades.Keys.Count; i++)
            {
                if (_currentSelectedGrenade.Prefab == _grenades.Values.ElementAt(i).Prefab)
                {
                    return i;
                }
            }

            throw new ArgumentOutOfRangeException("Index not found");
        }

        public void Select()
        {
            if (gameObject.activeInHierarchy)
            {
                SwitchGrenade();
            }
            SwitchGrenade();
            gameObject.SetActive(true);
        }

        public void Deselect()
        {
            gameObject.SetActive(false);
        }
    }
}