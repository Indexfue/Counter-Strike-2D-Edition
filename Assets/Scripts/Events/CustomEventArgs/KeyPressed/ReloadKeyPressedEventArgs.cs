using UnityEngine;

namespace Player
{
    public sealed class ReloadKeyPressedEventArgs : BaseEventArgs
    {
        public ReloadKeyPressedEventArgs(GameObject sender) : base(sender) { }
    }
}