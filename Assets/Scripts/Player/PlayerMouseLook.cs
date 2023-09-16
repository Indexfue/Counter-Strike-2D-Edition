using UnityEngine;

namespace Player
{
    public class PlayerMouseLook : MonoBehaviour
    {
        private void Start() => Cursor.lockState = CursorLockMode.Confined;

            private void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            //mousePosition.z = transform.position.z - Camera.main.transform.position.z;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            
            float angle = Vector2.Angle(Vector2.right, mousePosition - transform.position);
            transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePosition.y ? angle : -angle);
        }
    }
}