using UnityEngine;

namespace Player
{
    public sealed class ItemSelectKeyPressedEventArgs : BaseEventArgs
    {
        public float KeyCode { get; }

        public ItemSelectKeyPressedEventArgs(GameObject sender, float keyCode) : base(sender)
        {
            KeyCode = keyCode;
        }
    }
}