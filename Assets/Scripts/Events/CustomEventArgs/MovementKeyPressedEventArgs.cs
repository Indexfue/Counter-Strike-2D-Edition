using UnityEngine;

namespace Player
{
    public sealed class MovementKeyPressedEventArgs : BaseEventArgs
    {
        public Vector3 MovementDirection { get; }

        public MovementKeyPressedEventArgs(GameObject sender, Vector3 movementDirection) : base(sender)
        {
            MovementDirection = movementDirection;
        }
    }
}