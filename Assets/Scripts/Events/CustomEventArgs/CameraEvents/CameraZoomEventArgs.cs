using UnityEngine;

namespace Player
{
    public class CameraZoomEventArgs : BaseEventArgs
    {
        public float EndCameraOrthoSize { get; }
        public float ZoomTime { get; }
        
        public CameraZoomEventArgs(GameObject sender, float endCameraOrthoSize, float zoomTime) : base(sender)
        {
            EndCameraOrthoSize = endCameraOrthoSize;
            ZoomTime = zoomTime;
        }
    }
}