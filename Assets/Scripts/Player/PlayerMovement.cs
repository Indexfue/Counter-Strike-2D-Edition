using System;
using System.Collections;
using Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private StaminaComponent staminaComponent;
        
        private float _currentMovementSpeed;
        private Vector3 _movementDirection;
        private Rigidbody2D _rigidbody;
        private CharacterController _characterController;
        private readonly UnitVector _unitVector = new UnitVector(0, 0);

        private Coroutine _unitCounterCoroutine;

        public float CurrentMovementSpeed => _currentMovementSpeed;
        public float CurrentUnitSpeed => Math.Max(Math.Abs(_unitVector.XSpeedRate), Math.Abs(_unitVector.YSpeedRate));
        public Vector3 MovementDirection => _movementDirection;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _currentMovementSpeed = Configuration.BaseMovementSpeed;
        }

        private void OnEnable()
        {
            EventManager.Subscribe<MovementKeyPressedEventArgs>(OnMovement);
            EventManager.Subscribe<SprintKeyEventArgs>(OnSprintKeyPressed);
            _unitCounterCoroutine = StartCoroutine(UnitCounterRoutine());
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<MovementKeyPressedEventArgs>(OnMovement);
            EventManager.Unsubscribe<SprintKeyEventArgs>(OnSprintKeyPressed);
            StopCoroutine(_unitCounterCoroutine);
        }

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private IEnumerator UnitCounterRoutine()
        {
            while (true)
            {
                _unitVector.UpdateSpeed(_movementDirection);
                yield return new WaitForSecondsRealtime(0.005f);
            }
        }

        private void OnMovement(MovementKeyPressedEventArgs args) => _movementDirection = args.MovementDirection.normalized;

        private void OnSprintKeyPressed(SprintKeyEventArgs args)
        {
            if (!args.Cancel)
            {
                //HandleDash();

                if (_unitVector.YSpeedRate == 0 && _unitVector.XSpeedRate == 0)
                {
                    return;
                }

                _currentMovementSpeed = Configuration.BaseSprintSpeed;
                staminaComponent.HandleStaminaReduce();
            }

        }

        private void OnSprintKeyUnpressed(SprintKeyEventArgs args)
        {
            if (args.Cancel)
            {
                _currentMovementSpeed = Configuration.BaseMovementSpeed;
                staminaComponent.StopStaminaReduce();
            }
        }

        private void HandleDash()
        {
            if (staminaComponent.CurrentStamina > Configuration.DashStaminaReduce)
            {
                if (_movementDirection == Vector3.zero)
                {
                    _movementDirection = transform.TransformDirection(Vector3.forward);
                }
                staminaComponent.CurrentStamina -= Configuration.DashStaminaReduce;
                StartCoroutine(HandleDashRoutine());
                _movementDirection = Vector3.zero;
            }
        }

        private IEnumerator HandleDashRoutine()
        {
            _currentMovementSpeed = Configuration.BaseDashSpeed;
            yield return new WaitForSeconds(0.2f);
            _currentMovementSpeed = Configuration.BaseMovementSpeed;
        }

        private void MoveCharacter()
        {
            var direction = (Vector3.right * _unitVector.XSpeedRate) + (Vector3.forward * _unitVector.YSpeedRate);
            _characterController.Move(direction * _currentMovementSpeed);
        }
    }

    public class UnitVector
    {
        public readonly float UnitSpeedLimit;

        private float _baseUnitUpdateSpeedRate;
        private int _x;
        private int _y;

        public float UnitUpdateSpeedRate => _baseUnitUpdateSpeedRate;
        public int X => _x;
        public int Y => _y;
        public float XSpeedRate => _x / UnitSpeedLimit;
        public float YSpeedRate => _y / UnitSpeedLimit;

        public UnitVector(int x = 0, int y = 0)
        {
            _baseUnitUpdateSpeedRate = 0.0005f;
            UnitSpeedLimit = 15.0f;
            _x = x;
            _y = y;
        }
        
        public void UpdateSpeed(Vector3 direction)
        {
            Vector3 rawDirection = direction.normalized;
            //Vector2Int directionVector2 = new Vector2Int(Mathf.RoundToInt(rawDirection.x), Mathf.RoundToInt(rawDirection.z));
            int newX = rawDirection.x == 0 && X != 0 ? (int)(X * -1.0f / Mathf.Abs(X)) : Mathf.RoundToInt(rawDirection.x);
            int newY = rawDirection.z == 0 && Y != 0 ? (int)(Y * -1.0f / Mathf.Abs(Y)) : Mathf.RoundToInt(rawDirection.z);
            Vector2Int directionVector2 = new Vector2Int(newX, newY);
            
            /*if (direction == Vector3.zero)
            {
                directionVector2 = new Vector2Int(X != 0 ? (int)(X * -1.0f / Mathf.Abs(X)) : 0, Y != 0 ? (int)(Y * -1.0f / Mathf.Abs(Y)) : 0);
            }*/
            if (Math.Abs(X + directionVector2.x) <= UnitSpeedLimit)
                _x += directionVector2.x;
            if (Math.Abs(Y + directionVector2.y) <= UnitSpeedLimit)
                _y += directionVector2.y;
        }
    }
}