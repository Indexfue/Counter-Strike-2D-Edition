using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(Weapon))]
    public class PlayerInventory : MonoBehaviour
    {
        private IInventoryItem _currentInventoryItem;

        private Dictionary<InventoryItems, IInventoryItem> _inventoryItems;

        private void Start()
        {
            _inventoryItems = new Dictionary<InventoryItems, IInventoryItem>()
            {
                [InventoryItems.Primary] = null,
                [InventoryItems.Secondary] = null,
                [InventoryItems.Melee] = null
            };
        } 

        private void OnEnable()
        {
            InputHandler.WeaponPickKeyPressed += ChangeCurrentItem;
        }

        private void OnDisable()
        {
            InputHandler.WeaponPickKeyPressed -= ChangeCurrentItem;
        }

        //TODO: Rewrite this piece of sss... something bad
        private void ChangeCurrentItem(float keyPressed)
        {
            InventoryItems inventoryItem = (InventoryItems)keyPressed;
            ChangeCurrentItem(inventoryItem);
        }

        public void ChangeCurrentItem(InventoryItems inventoryItem)
        {
            if (_inventoryItems.TryGetValue(inventoryItem, out IInventoryItem item))
            {
                if (item != null)
                {
                    _currentInventoryItem?.Deselect(); // Деактивируем текущий предмет
                    _currentInventoryItem = item;
                    _currentInventoryItem.Select(); // Активируем новый предмет
                }
            }
        }

        public void AddItem(InventoryItems inventoryItem, IInventoryItem item)
        {
            if (_inventoryItems.TryGetValue(inventoryItem, out IInventoryItem currentItem))
            {
                if (currentItem == null)
                {
                    _inventoryItems[inventoryItem] = item;
                }
            }
        }

        public void DropItem()
        {
            _currentInventoryItem.Deselect();
            _currentInventoryItem = null;
            
            if (_inventoryItems[InventoryItems.Primary] != null)
            {
                ChangeCurrentItem(InventoryItems.Primary);
                return;
            }
            
            if (_inventoryItems[InventoryItems.Secondary] != null)
            {
                ChangeCurrentItem(InventoryItems.Secondary);
                return;
            }
            
            ChangeCurrentItem(InventoryItems.Melee);
        }
    }

    public enum InventoryItems
    {
        Primary = 1,
        Secondary = 2,
        Melee = 3,
    }
}