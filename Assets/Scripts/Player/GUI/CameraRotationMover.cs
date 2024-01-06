using Cinemachine;
using UnityEngine;

namespace Player.GUI
{
    public class CameraRotationMover : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        
        public void Rotate(float angle, Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                return;
            }
            
            _cinemachineVirtualCamera.transform.Rotate(direction, angle);
        }
    }
}