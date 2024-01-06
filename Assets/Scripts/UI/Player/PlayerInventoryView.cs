using System;
using System.Collections.Generic;
using Player;
using Player.UI.Weapons;
using UnityEngine;

namespace UI.Player
{
    public class PlayerInventoryView : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryItemViewPrefab;
        
        private readonly List<PlayerInventoryItemView> _itemViews = new List<PlayerInventoryItemView>();
        private PlayerInventoryItemView _currentItem;

        public void Initialize()
        {
            foreach(InventoryItemType type in Enum.GetValues(typeof(InventoryItemType)))
            {
                GameObject obj = Instantiate(inventoryItemViewPrefab);
                
                PlayerInventoryItemView UIElement = obj.GetComponent<PlayerInventoryItemView>();
                UIElement.transform.SetParent(transform, false);
                UIElement.gameObject.SetActive(false);
                UIElement.Initialize(type);
                
                _itemViews.Add(UIElement);
            }
        }
        
        public void UpdateView(ItemViewData data)
        {
            PlayerInventoryItemView itemView = _itemViews.Find(e => e.Type == data.Type);

            if (itemView != null)
            {
                itemView.UpdateView(data, null);
                itemView.gameObject.SetActive(true);
            }
        }

        public void SetCurrentItemView(InventoryItemType type)
        {
            _currentItem?.SetUnselected();
            _currentItem = _itemViews.Find(e => e.Type == type);
            _currentItem.SetSelected();
        }
    }
}