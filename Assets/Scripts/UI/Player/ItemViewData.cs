using System;
using UnityEngine;

namespace Player.UI.Weapons
{
    [Serializable]
    public struct ItemViewData
    {
        [SerializeField] private string title;
        [SerializeField] private Texture icon;
        [SerializeField] private InventoryItemType type;

        public string Title => title;
        public Texture Icon => icon;
        public InventoryItemType Type => type;

        public ItemViewData(string title, Texture icon, InventoryItemType type)
        {
            this.title = title;
            this.icon = icon;
            this.type = type;
        }
    }
}