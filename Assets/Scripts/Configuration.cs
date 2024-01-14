using UnityEngine;

namespace Player
{
    public static class Configuration
    {
        #region Camera Variables

        public static readonly Vector3 BaseCameraOffsetKeyPressed = new Vector3(0f, 8f, 0f);
        public static readonly Vector3 BaseCameraOffsetKeyUnpressed = Vector3.zero;
        public static readonly float BaseCameraOffsetMoveTimeKeyPressed = 0.75f;
        public static readonly float BaseCameraOffsetMoveTimeKeyUnpressed = 0.5f;

        #endregion
    }
}