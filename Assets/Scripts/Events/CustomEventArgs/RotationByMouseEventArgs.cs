using UnityEngine;

namespace Player
{
    public sealed class RotationByMouseEventArgs : BaseEventArgs
    {
        public RotationByMouseEventArgs(GameObject sender) : base(sender) { }
    }
}