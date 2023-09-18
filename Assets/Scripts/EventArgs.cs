using System;
using UnityEngine;

namespace Player
{
    public sealed class FireKeyPressedEventArgs : EventArgs { }
    public sealed class FireKeyUnpressedEventArgs : EventArgs { }
    public sealed class ReloadKeyPressedEventArgs : EventArgs { }

    public sealed class WeaponSelectKeyPressedEventArgs : EventArgs
    {
        public float KeyCode { get; }

        public WeaponSelectKeyPressedEventArgs(float keyCode) => KeyCode = keyCode;
    }

    public sealed class MovementKeyPressedEventArgs : EventArgs
    {
        public Vector3 MovementDirection { get; }

        public MovementKeyPressedEventArgs(Vector3 movementDirection) => MovementDirection = movementDirection;
    }
}