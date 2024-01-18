using System;
using Cinemachine;
using System.Collections;
using Player;
using UnityEngine;

namespace Bonehead.Combat.Player.UI
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraZoom : MonoBehaviour
    {
        private IEnumerator _coroutine;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        public float BeginCameraOrthoSize { get; private set; }
        
        public void HandleCameraZoom(float endCameraOrthoSize, float zoomTime)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = CameraZoomRoutine(endCameraOrthoSize, zoomTime);
            StartCoroutine(_coroutine);
        }

        private void HandleCameraZoom(CameraZoomEventArgs args) =>
            HandleCameraZoom(args.EndCameraOrthoSize, args.ZoomTime);

        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            BeginCameraOrthoSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
        }

        private void OnEnable()
        {
            EventManager.Subscribe<CameraZoomEventArgs>(HandleCameraZoom);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<CameraZoomEventArgs>(HandleCameraZoom);
        }

        private IEnumerator CameraZoomRoutine(float endCameraOrthoSize, float zoomTime)
        {
            float currentCameraOrthoSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
            float cameraOrthoSizeDifference = Mathf.Abs(currentCameraOrthoSize - endCameraOrthoSize);

            for (int i = 0; i <= 100; i++)
            {
                float nextOrthograpicSize = Mathf.Lerp(currentCameraOrthoSize, endCameraOrthoSize, Mathf.Pow(i / 100f, zoomTime));
                _cinemachineVirtualCamera.m_Lens.OrthographicSize = nextOrthograpicSize;

                yield return new WaitForSeconds(zoomTime / 100f);
            }

            _cinemachineVirtualCamera.m_Lens.OrthographicSize = endCameraOrthoSize;
        }
    }
}