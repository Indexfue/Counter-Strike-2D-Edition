using System;
using UnityEngine;

namespace Player
{
    public sealed class FireKeyPressedEventArgs : EventArgs { }
    public sealed class FireKeyUnpressedEventArgs : EventArgs { }
    
    public sealed class SecondaryFireKeyPressedEventArgs : EventArgs { }
    public sealed class SecondaryFireKeyUnpressedEventArgs : EventArgs { }
    public sealed class ReloadKeyPressedEventArgs : EventArgs { }

    public sealed class ItemSelectKeyPressedEventArgs : EventArgs
    {
        public float KeyCode { get; }

        public ItemSelectKeyPressedEventArgs(float keyCode) => KeyCode = keyCode;
    }

    public sealed class MovementKeyPressedEventArgs : EventArgs
    {
        public Vector3 MovementDirection { get; }

        public MovementKeyPressedEventArgs(Vector3 movementDirection) => MovementDirection = movementDirection;
    }

    public sealed class CameraOffsetEventArgs : EventArgs
    {
        public Vector3 OffsetValue { get; }
        public float MoveTime { get; }

        public CameraOffsetEventArgs(Vector3 baseOffsetValue, float baseMoveTime, Vector3 offsetValue = default(Vector3), float moveTime = 0f)
        {
            if (offsetValue == default(Vector3))
            {
                offsetValue = baseOffsetValue;
            }

            if (moveTime == 0f)
            {
                moveTime = baseMoveTime;
            }

            OffsetValue = offsetValue;
            MoveTime = moveTime;
        }
    }
}