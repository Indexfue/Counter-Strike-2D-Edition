using UnityEngine;

namespace Player
{
    public abstract class CancelEventArgs : BaseEventArgs
    {
        public bool Cancel { get; }

        protected CancelEventArgs(GameObject sender, bool cancel = false) : base(sender)
        {
            Cancel = cancel;
        }
    }
}