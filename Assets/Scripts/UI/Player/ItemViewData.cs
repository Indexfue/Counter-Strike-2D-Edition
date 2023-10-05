using System;
using UnityEngine;

namespace Player.UI.Weapons
{
    [Serializable]
    public struct ItemViewData
    {
        [SerializeField] private string _title;
        [SerializeField] private Texture _icon;
        [SerializeField] private InventoryItemType _type;

        public string Title => _title;
        public Texture Icon => _icon;
        public InventoryItemType Type => _type;

        public ItemViewData(string title, Texture icon, InventoryItemType type)
        {
            _title = title;
            _icon = icon;
            _type = type;
        }
    }
}