using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using Utilities.Grenades;
using Weapons;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<InventoryItemType, WeaponSettings> weaponsSettings;
        

        [SerializeField] private GrenadeInventory grenadeInventory;
        [SerializeField] private Weapon weaponObject;

        private IInventoryItem _currentInventoryItem;

        public IInventoryItem CurrentInventoryItem
        {
            get => _currentInventoryItem;
            set
            {
                if (value == CurrentInventoryItem)
                {
                    return;
                }
                _currentInventoryItem?.Deselect();
                _currentInventoryItem = value;
                _currentInventoryItem.Select();
            }
        }

        private void Start()
        {
            weaponsSettings = new()
            {
                [InventoryItemType.Primary] = null,
                [InventoryItemType.Secondary] = null,
                [InventoryItemType.Melee] = null,
            };
            
            DeselectAll();
            SelectFirst();
        }
        
        private void OnEnable()
        {
            EventManager.Subscribe<ItemSelectKeyPressedEventArgs>(OnItemSelectKeyPressed);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<ItemSelectKeyPressedEventArgs>(OnItemSelectKeyPressed);
        }
        
        private void OnItemSelectKeyPressed(ItemSelectKeyPressedEventArgs args)
        {
            InventoryItemType inventoryItemType = (InventoryItemType)args.KeyCode;
            SelectInventoryItem(inventoryItemType);
        }

        // TODO: Rewrite this awful thing
        public void SelectInventoryItem(InventoryItemType inventoryItemType)
        {
            if (inventoryItemType == InventoryItemType.Grenade)
            {
                if (grenadeInventory.IsEmpty)
                {
                    return;
                }
                CurrentInventoryItem = grenadeInventory;
            }
            else
            {
                if (weaponsSettings[inventoryItemType] == null)
                {
                    return;
                }
                weaponObject.Settings = weaponsSettings[inventoryItemType];
                CurrentInventoryItem = weaponObject;
            }
        }

        public void SelectFirst()
        {
            WeaponSettings settings = weaponsSettings.Values.First(e => e is not null);
            if (settings != null)
            {
                weaponObject.Settings = settings;
                return;
            }

            SelectInventoryItem(InventoryItemType.Grenade);
        }

        public void DeselectAll()
        {
            weaponObject.Deselect();
            grenadeInventory.Deselect();
        }

        public void AddInventoryItem(InventoryItemType inventoryItemType, IInventoryItem item)
        {
            
        }

        public void RemoveInventoryItem()
        {
            
        }
    }
}