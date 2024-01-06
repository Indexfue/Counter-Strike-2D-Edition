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
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text binding;
        [SerializeField] private RawImage icon;
        
        public InventoryItemType Type { get; private set; }

        public void Initialize(InventoryItemType type)
        {
            Type = type;
            SetUnselected();
        }

        public void UpdateView(ItemViewData data, [CanBeNull] string binding)
        {
            if (data.Type != Type) return;
            
            title.text = data.Title;
            icon.texture = data.Icon;
            this.binding.text = binding ?? this.binding.text;
        }

        public void SetSelected()
        {
            title.alpha = 1;
        }

        public void SetUnselected()
        {
            title.alpha = 0;
        }
    }
}