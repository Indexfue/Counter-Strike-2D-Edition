using UnityEngine;

namespace Player
{
    public sealed class CameraOffsetEventArgs : BaseEventArgs
    {
        public Vector3 OffsetValue { get; }
        public float MoveTime { get; }

        public CameraOffsetEventArgs(GameObject sender, Vector3 baseOffsetValue, float baseMoveTime, Vector3 offsetValue = default(Vector3), float moveTime = 0f) : base(sender)
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