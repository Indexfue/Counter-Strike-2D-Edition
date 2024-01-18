using UnityEngine;

namespace Player
{
    public static class Configuration
    {
        #region Camera Variables

        // Camera offset
        public static readonly Vector3 BaseCameraOffsetKeyPressed = new Vector3(0f, 8f, 0f);
        public static readonly Vector3 BaseCameraOffsetKeyUnpressed = Vector3.zero;
        public static readonly float BaseCameraOffsetMoveTimeKeyPressed = 0.75f;
        public static readonly float BaseCameraOffsetMoveTimeKeyUnpressed = 0.5f;
        
        // Camera Zoom
        public static readonly float BaseCameraZoomOrthoSize = 12.15f;
        public static readonly float AimCameraZoomOrthoSize = 9.5f;
        
        public static readonly Vector3 BaseCameraRotation = new Vector3(90f, 0f, 0f);

        #endregion

        #region Component Variables

        // Stamina
        public static readonly float BaseStaminaValue = 100f;
        public static readonly float StaminaPerLevel = 20f;
        public static readonly float StaminaReducingPerSecond = 20f;
        public static readonly float DashStaminaReduce = 20f;
        public static readonly float StaminaRegenerationPerSecond = 25f;
        public static readonly float StaminaRegerationStartTimer = 2.5f;
        // End Stamina

        #endregion

        #region Player Variables

        // Speed
        public static readonly float BaseMovementSpeed = 0.1f;
        public static readonly float BaseSprintSpeed = 0.15f;
        public static readonly float BaseDashSpeed = 0.25f;
        
        //Dash
        public static readonly float DashDuration = 0.2f;
        public static readonly float DashCooldown = 1f;
        
        //Aim
        public static readonly float AimRadius = 15f;

        #endregion
    }
}