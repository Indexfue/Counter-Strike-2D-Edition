using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float mouseSensitivity = 1f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            var x = Input.GetAxis("Mouse X");
            var rotateValue = new Vector3(0, x * -1, 0);

            transform.eulerAngles -= rotateValue;
            transform.eulerAngles +=  rotateValue * mouseSensitivity * Time.deltaTime;
        }
    }
}