using System;
using JetBrains.Annotations;
using Player;
using Player.UI.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player
{
    public class PlayerInventoryItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _binding;
        [SerializeField] private RawImage _icon;
        
        public InventoryItemType Type { get; private set; }

        public void Initialize(InventoryItemType type)
        {
            Type = type;
            SetUnselected();
        }

        public void UpdateView(ItemViewData data, [CanBeNull] string binding)
        {
            if (data.Type != Type) return;
            
            _title.text = data.Title;
            _icon.texture = data.Icon;
            _binding.text = binding ?? _binding.text;
        }

        public void SetSelected()
        {
            _title.alpha = 1;
        }

        public void SetUnselected()
        {
            _title.alpha = 0;
        }
    }
}