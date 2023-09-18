using UnityEngine;

namespace Player
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _mouseSensitivity = 1f;
        [SerializeField] private Camera _camera;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            var x = Input.GetAxis("Mouse X");
            var _rotateValue = new Vector3(0, x * -1, 0);

            transform.eulerAngles -= _rotateValue;
            transform.eulerAngles +=  _rotateValue * _mouseSensitivity * Time.deltaTime;
            _camera.transform.eulerAngles = new Vector3(_camera.transform.eulerAngles.x, transform.eulerAngles.y, _camera.transform.eulerAngles.z);
            _camera.transform.position =
                new Vector3(transform.position.x, _camera.transform.position.y, transform.position.z);
        }
    }
}