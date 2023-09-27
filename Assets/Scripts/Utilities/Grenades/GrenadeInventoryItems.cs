using System;
using System.Collections.Generic;
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
                [GrenadeType.Flash] = new GrenadeInventoryItem(Resources.Load("Utilities/Grenades/Flashbang Grenade").GameObject(), 2),
                [GrenadeType.Explosion] = new GrenadeInventoryItem(Resources.Load("Utilities/Grenades/Explosion Grenade").GameObject(), 1),
                [GrenadeType.Smoke] = new GrenadeInventoryItem(Resources.Load("Utilities/Grenades/Smoke Grenade").GameObject(), 1),
                [GrenadeType.Fire] = new GrenadeInventoryItem(Resources.Load("Utilities/Grenades/Fire Grenade").GameObject(), 1)
            };

            _currentSelectedGrenade = _grenades[GrenadeType.Smoke];
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
            
            if (_leftClicked && _rightClicked)
                UseSelectedGrenade(GrenadeFlightMode.Medium);
            else if (_leftClicked)
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

        public void Select()
        {
            if (gameObject.activeSelf)
            {
                //_currentSelectedGrenade
            }
            
            gameObject.SetActive(true);
        }

        public void Deselect()
        {
            gameObject.SetActive(false);
        }
    }
}