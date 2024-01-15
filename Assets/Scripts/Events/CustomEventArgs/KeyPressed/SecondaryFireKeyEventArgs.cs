using UnityEngine;

namespace Player
{
    public sealed class SecondaryFireKeyEventArgs : CancelEventArgs
    {
        public SecondaryFireKeyEventArgs(GameObject sender, bool cancel = false) : base(sender, cancel) { }
    }
}