using System;
using System.Collections;
using UnityEngine;

namespace Player.GUI
{
    public class CameraOffsetMover : MonoBehaviour
    {
        private IEnumerator _coroutine;
        private CinemachineCameraOffset _cinemachineCameraOffset;
        
        public void HandleCameraOffset(CameraOffsetEventArgs args)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = CameraOffsetRoutine(args.OffsetValue, args.MoveTime);
            StartCoroutine(_coroutine);
        }

        private void Awake()
        {
            _cinemachineCameraOffset = GetComponent<CinemachineCameraOffset>();
        }

        private void OnEnable()
        {
            EventManager.Subscribe<CameraOffsetEventArgs>(HandleCameraOffset);
        }
        
        private void OnDisable()
        {
            EventManager.Unsubscribe<CameraOffsetEventArgs>(HandleCameraOffset);
        }

        private IEnumerator CameraOffsetRoutine(Vector3 endCameraOffset, float moveTime)
        {
            Vector3 currentCameraOffset = _cinemachineCameraOffset.m_Offset;

            for (int i = 0; i <= 100; i++)
            {
                Vector3 nextOffset = Vector3.Lerp(currentCameraOffset, endCameraOffset, (i / 100f + Mathf.Pow(i / 100f, 1 / Mathf.Exp(1))) / Mathf.Pow(i / 100f, -1));
                _cinemachineCameraOffset.m_Offset = nextOffset;
                yield return new WaitForSeconds(moveTime / 100f);
            }

            _cinemachineCameraOffset.m_Offset = endCameraOffset;
        }
    }
}