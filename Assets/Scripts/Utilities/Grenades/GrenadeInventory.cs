using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utilities.Grenades
{
    public class GrenadeInventory : MonoBehaviour, IInventoryItem
    {
        [SerializeField] private Transform throwPosition;
        [SerializeField] private SerializableDictionary<GrenadeType, GrenadeInventoryItem> grenades;
        [SerializeField] private GrenadeInventoryItem currentSelectedGrenade;

        private bool _leftClicked;
        private bool _rightClicked;

        public GrenadeInventoryItem CurrentSelectedGrenade => currentSelectedGrenade;

        public bool IsEmpty => grenades.Values.All(e => e.CurrentCapacity == 0);

        private void Start()
        {
            grenades = new SerializableDictionary<GrenadeType, GrenadeInventoryItem>()
            {
                [GrenadeType.Flash] = new(Resources.Load("Utilities/Grenades/Flashbang Grenade").GameObject(), GrenadeType.Flash, 2),
                [GrenadeType.Explosion] = new(Resources.Load("Utilities/Grenades/Explosion Grenade").GameObject(), GrenadeType.Explosion, 1),
                [GrenadeType.Smoke] = new(Resources.Load("Utilities/Grenades/Smoke Grenade").GameObject(), GrenadeType.Smoke, 1),
                [GrenadeType.Fire] = new(Resources.Load("Utilities/Grenades/Fire Grenade").GameObject(), GrenadeType.Fire, 1)
            };

            currentSelectedGrenade = grenades[GrenadeType.Flash];
            SwitchGrenade();
        }

        private void OnEnable()
        {
            EventManager.Subscribe<FireKeyEventArgs>(OnLeftButtonMousePerformed);
            EventManager.Subscribe<SecondaryFireKeyEventArgs>(OnRightButtonMousePerformed);
            EventManager.Subscribe<FireKeyEventArgs>(OnButtonMouseCanceled);
            EventManager.Subscribe<SecondaryFireKeyEventArgs>(OnButtonMouseCanceled);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<FireKeyEventArgs>(OnLeftButtonMousePerformed);
            EventManager.Unsubscribe<SecondaryFireKeyEventArgs>(OnRightButtonMousePerformed);
            EventManager.Unsubscribe<FireKeyEventArgs>(OnButtonMouseCanceled);
            EventManager.Unsubscribe<SecondaryFireKeyEventArgs>(OnButtonMouseCanceled);
        }

        private void OnLeftButtonMousePerformed(FireKeyEventArgs args) => _leftClicked = true;

        private void OnRightButtonMousePerformed(SecondaryFireKeyEventArgs args) => _rightClicked = true;

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
            currentSelectedGrenade.Use(throwPosition, flightMode);
        }

        private void SwitchGrenade()
        {
            grenades[currentSelectedGrenade.GrenadeType] = currentSelectedGrenade;
            currentSelectedGrenade = SetNextGrenade(currentSelectedGrenade);
        }

        private GrenadeInventoryItem SetNextGrenade(GrenadeInventoryItem currentSelectedGrenade)
        {
            //TODO: CurrentCapacity = 0
            int currentGrenadeIndex = GetCurrentGrenadeIndex();

            for (int i = currentGrenadeIndex + 1; i <= currentGrenadeIndex + grenades.Keys.Count; i++)
            {
                GrenadeInventoryItem nextGrenade = grenades.Values.ElementAt(i % grenades.Keys.Count);

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
            for (int i = 0; i < grenades.Keys.Count; i++)
            {
                if (currentSelectedGrenade.Prefab == grenades.Values.ElementAt(i).Prefab)
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