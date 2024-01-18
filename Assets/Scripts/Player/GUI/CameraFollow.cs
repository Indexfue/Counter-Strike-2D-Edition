using System;
using Cinemachine;
using UnityEngine;

namespace Player.GUI
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform transformToFollow;

        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        public void SetTransformToFollow(CameraFollowEventArgs args)
        {
            transformToFollow = args.ToFollow;

            if (_cinemachineVirtualCamera != null)
            {
                _cinemachineVirtualCamera.Follow = transformToFollow;
            }
        }
        
        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cinemachineVirtualCamera.Follow = transformToFollow;
        }

        private void OnEnable()
        {
            EventManager.Subscribe<CameraFollowEventArgs>(SetTransformToFollow);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<CameraFollowEventArgs>(SetTransformToFollow);
        }
    }
}