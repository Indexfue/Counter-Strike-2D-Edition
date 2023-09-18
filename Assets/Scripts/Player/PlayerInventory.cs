﻿using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private InventoryDictionary _inventoryItems;
        private IInventoryItem _currentInventoryItem;

        private void Start()
        {
            _inventoryItems = new InventoryDictionary()
            {
                [InventoryItemType.Primary] = GetComponentInChildren<PrimaryWeapon>(),
                [InventoryItemType.Secondary] = GetComponentInChildren<SecondaryWeapon>(),
                [InventoryItemType.Melee] = GetComponentInChildren<MeleeWeapon>()
            };
        } 

        private void OnEnable()
        {
            EventManager.Subscribe<WeaponSelectKeyPressedEventArgs>(ChangeCurrentItem);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<WeaponSelectKeyPressedEventArgs>(ChangeCurrentItem);
        }
        
        private void ChangeCurrentItem(WeaponSelectKeyPressedEventArgs args)
        {
            InventoryItemType inventoryItemType = (InventoryItemType)args.KeyCode;
            ChangeCurrentItem(inventoryItemType);
            Debug.Log(inventoryItemType);
        }

        public void ChangeCurrentItem(InventoryItemType inventoryItemType)
        {
            if (_inventoryItems.TryGetValue(inventoryItemType, out IInventoryItem item))
            {
                if (item != null)
                {
                    _currentInventoryItem?.Deselect();
                    _currentInventoryItem = item;
                    _currentInventoryItem.Select();
                }
            }
        }

        public void AddItem(InventoryItemType inventoryItemType, IInventoryItem item)
        {
            if (_inventoryItems.TryGetValue(inventoryItemType, out IInventoryItem currentItem))
            {
                if (currentItem == null)
                {
                    _inventoryItems[inventoryItemType] = item;
                }
            }
        }

        public void DropItem()
        {
            _currentInventoryItem.Deselect();
            _currentInventoryItem = null;
            
            if (_inventoryItems[InventoryItemType.Primary] != null)
            {
                ChangeCurrentItem(InventoryItemType.Primary);
                return;
            }
            
            if (_inventoryItems[InventoryItemType.Secondary] != null)
            {
                ChangeCurrentItem(InventoryItemType.Secondary);
                return;
            }
            
            ChangeCurrentItem(InventoryItemType.Melee);
        }
    }

    public enum InventoryItemType
    {
        Primary = 1,
        Secondary = 2,
        Melee = 3,
    }
}