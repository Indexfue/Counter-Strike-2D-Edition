using UnityEngine;

namespace Player
{
    public class SprintKeyEventArgs : CancelEventArgs
    {
        public SprintKeyEventArgs(GameObject sender, bool cancel = false) : base(sender, cancel) { }
    }
}