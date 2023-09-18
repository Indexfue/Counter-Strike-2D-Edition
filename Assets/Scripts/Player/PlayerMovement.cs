using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _movementSpeed;
        private Vector3 _movementDirection;
        private Rigidbody2D _rigidbody;
        private CharacterController _characterController;

        public float MovementSpeed => _movementSpeed;
        public Vector3 MovementDirection => _movementDirection;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            EventManager.Subscribe<MovementKeyPressedEventArgs>(OnMovement);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<MovementKeyPressedEventArgs>(OnMovement);
        }

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void OnMovement(MovementKeyPressedEventArgs args) => _movementDirection = args.MovementDirection.normalized;

        private void MoveCharacter()
        {
            var rawDirection = (transform.right * _movementDirection.x) + (transform.forward * _movementDirection.z);
            _characterController.Move(rawDirection.normalized * _movementSpeed);
        }
    }
}