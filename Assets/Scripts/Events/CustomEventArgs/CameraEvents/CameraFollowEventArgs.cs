using UnityEngine;

namespace Player
{
    public class CameraFollowEventArgs : BaseEventArgs
    {
        public Transform ToFollow { get; }
        
        public CameraFollowEventArgs(GameObject sender, Transform toFollow) : base(sender)
        {
            ToFollow = toFollow;
        }
    }
}