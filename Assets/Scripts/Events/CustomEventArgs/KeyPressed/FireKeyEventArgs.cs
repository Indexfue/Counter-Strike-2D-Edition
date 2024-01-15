using UnityEngine;

namespace Player
{
    public sealed class FireKeyEventArgs : CancelEventArgs
    {
        public FireKeyEventArgs(GameObject sender, bool cancel = false) : base(sender, cancel) { }
    }
}