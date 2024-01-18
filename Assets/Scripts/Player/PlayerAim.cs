using System;
using Player.PlayerEvents;
using Player.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private AimView viewPrefab;
        [SerializeField] private GameObject aimObjectPrefab;
        [SerializeField] private LayerMask aimMask;
        // To debug
        [SerializeField] private Transform toFollow;

        private GameObject _aimObject;
        private Canvas _canvas;
        
        private void Start() => Initialize();
        
        private void OnEnable()
        {
            EventManager.Subscribe<SecondaryFireKeyEventArgs>(OnSecondaryFireKeyPressed);
            EventManager.Subscribe<SecondaryFireKeyEventArgs>(OnSecondaryFireKeyUnpressed);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<SecondaryFireKeyEventArgs>(OnSecondaryFireKeyPressed);
            EventManager.Unsubscribe<SecondaryFireKeyEventArgs>(OnSecondaryFireKeyUnpressed);
        }

        private void Update()
        {
            UpdateAimObjectPosition();
        }

        private void OnSecondaryFireKeyPressed(SecondaryFireKeyEventArgs args)
        {
            if (args.Cancel)
            {
                return;
            }

            _aimObject = Instantiate(aimObjectPrefab, transform, true);
            
            EventManager.RaiseEvent(new CameraFollowEventArgs(gameObject, _aimObject.transform));
            EventManager.RaiseEvent(new CameraZoomEventArgs(gameObject, Configuration.AimCameraZoomOrthoSize, 0.5f));
        }

        private void OnSecondaryFireKeyUnpressed(SecondaryFireKeyEventArgs args)
        {
            if (!args.Cancel)
            {
                return;
            }

            Destroy(_aimObject);
            EventManager.RaiseEvent(new CameraFollowEventArgs(gameObject, transform));
            EventManager.RaiseEvent(new CameraZoomEventArgs(gameObject, Configuration.BaseCameraZoomOrthoSize, 0.5f));
        }

        private void UpdateAimObjectPosition()
        {
            if (_aimObject == null)
            {
                return;
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimMask))
            {
                if (Vector3.Distance(hit.point, transform.position) < Configuration.AimRadius)
                {
                    _aimObject.transform.position = hit.point;
                }
            }
        }

        private void Initialize()
        {
            // canvas? ui?
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_aimObject != null)
            {
                Gizmos.DrawWireSphere(transform.position, Configuration.AimRadius);
            }
        }

#endif
    }
}