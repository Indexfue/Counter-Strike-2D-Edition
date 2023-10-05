using System;
using System.Linq;
using Interfaces;
using UI.Player;
using UnityEditor;
using UnityEngine;
using Utilities.Grenades;
using Weapons;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private PlayerInventoryView _view;
        [SerializeField] private SerializableDictionary<InventoryItemType, IInventoryItem> _inventoryItems;
        private IInventoryItem _currentInventoryItem;
        
        public void Start()
        {
            _inventoryItems = new SerializableDictionary<InventoryItemType, IInventoryItem>()
            {
                [InventoryItemType.Primary] = GetComponentInChildren<PrimaryWeapon>(),
                [InventoryItemType.Secondary] = GetComponentInChildren<SecondaryWeapon>(),
                [InventoryItemType.Melee] = GetComponentInChildren<MeleeWeapon>(),
                [InventoryItemType.Grenade] = GetComponentInChildren<GrenadeInventoryItems>()
            };
            _view.Initialize();

            DeselectAll();
            SetStartWeapon();
            OnInventoryUpdate();
        }

        private void OnEnable()
        {
            EventManager.Subscribe<ItemSelectKeyPressedEventArgs>(ChangeCurrentItem);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<ItemSelectKeyPressedEventArgs>(ChangeCurrentItem);
        }

        private void OnInventoryUpdate()
        {
            foreach (var item in _inventoryItems.Values)
            {
                if (item is Weapon weapon)
                {
                    if (weapon.Settings == null) continue;

                    _view.UpdateView(weapon.Settings.ViewData);
                }
            }
        }
        
        private void ChangeCurrentItem(ItemSelectKeyPressedEventArgs args)
        {
            InventoryItemType inventoryItemType = (InventoryItemType)args.KeyCode;
            ChangeCurrentItem(inventoryItemType);
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
                    
                    _view.SetCurrentItemView(inventoryItemType);
                }
            }
        }
        
        private void SetStartWeapon()
        {
            foreach (var inventoryItem in _inventoryItems.Values)
            {
                if (inventoryItem is Weapon weapon)
                {
                    if (weapon.Settings != null)
                    {
                        inventoryItem.Select();
                        _view.SetCurrentItemView(weapon.Settings.ViewData.Type);
                        return;
                    }
                }
                else
                {
                    throw new Exception("Need to add at least 1 weapon settings (Primary/Secondary/Melee)");
                }
            }
        }

        private void DeselectAll()
        {
            foreach (var inventoryItem in _inventoryItems.Values)
            {
                if (inventoryItem != null)
                {
                    inventoryItem.Deselect();
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
            if (_currentInventoryItem == _inventoryItems[InventoryItemType.Melee])
                return;
            
            //_view.UpdateView(_inventoryItems.FirstOrDefault(e => e.Value == _currentInventoryItem).Key);
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
}