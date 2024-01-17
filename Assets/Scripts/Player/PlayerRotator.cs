using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private LayerMask cursorAimMask;

        public bool IsMouseRotationAllowed { get; set; } = true;

        public void OnRotationByMouse()
        {
            if (!IsMouseRotationAllowed)
            {
                return;
            }
        
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, cursorAimMask))
            {
                Vector3 lookingPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(lookingPosition);
            }
        }

        private void Update()
        {
            OnRotationByMouse();
        }
    }
}