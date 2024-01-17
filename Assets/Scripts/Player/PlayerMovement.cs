using System;
using System.Collections;
using Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(StaminaComponent))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        private StaminaComponent _staminaComponent;
        private Rigidbody2D _rigidbody;
        private CharacterController _characterController;

        private Vector3 _movementInput;
        private Vector3 _movementDirection;
        private readonly UnitVector _unitVector = new(0, 0);
        
        private float _currentMovementSpeed;
        private float _dashCooldownTimer;
        private bool _isDashPerforming;

        private Coroutine _unitCounterCoroutine;
        private Coroutine _dashPerformingCoroutine;
        private Coroutine _sprintCoroutine;
        
        public float CurrentUnitSpeed => Math.Max(Math.Abs(_unitVector.XSpeedRate), Math.Abs(_unitVector.YSpeedRate));
        public Vector3 MovementInput => _movementInput;
        public Vector3 MovementDirection => _movementDirection;
        public bool IsDashCooldown => _dashCooldownTimer > 0f;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _staminaComponent = GetComponent<StaminaComponent>();
            _currentMovementSpeed = Configuration.BaseMovementSpeed;
        }

        private void OnEnable()
        {
            EventManager.Subscribe<MovementKeyPressedEventArgs>(OnMovement);
            EventManager.Subscribe<SprintKeyEventArgs>(OnSprintKeyPressed);
            EventManager.Subscribe<SprintKeyEventArgs>(OnSprintKeyUnpressed);
            
            _unitCounterCoroutine = StartCoroutine(UnitCounterRoutine());
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<MovementKeyPressedEventArgs>(OnMovement);
            EventManager.Unsubscribe<SprintKeyEventArgs>(OnSprintKeyPressed);
            EventManager.Unsubscribe<SprintKeyEventArgs>(OnSprintKeyUnpressed);
            
            StopCoroutine(_unitCounterCoroutine);
        }

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void Update()
        {
            if (IsDashCooldown)
                _dashCooldownTimer -= Time.deltaTime;

            if (_staminaComponent.CurrentStamina == 0)
                _currentMovementSpeed = Configuration.BaseMovementSpeed;
        }

        private IEnumerator UnitCounterRoutine()
        {
            while (true)
            {
                _unitVector.UpdateSpeed(_movementInput);
                yield return new WaitForSecondsRealtime(0.005f);
            }
        }

        private void OnMovement(MovementKeyPressedEventArgs args) => _movementInput = args.MovementDirection.normalized;

        private void OnSprintKeyPressed(SprintKeyEventArgs args)
        {
            if (!args.Cancel)
            {
                PerformDash();
                
                if (_unitVector.YSpeedRate == 0 && _unitVector.XSpeedRate == 0)
                    return;

                _sprintCoroutine = StartCoroutine(SprintRoutine());
            }
        }

        private IEnumerator SprintRoutine()
        {
            if (_staminaComponent.CurrentStamina == 0f)
                yield return null;
            
            _staminaComponent.HandleStaminaReduce();
            
            while (_staminaComponent.CurrentStamina > 0f)
            {
                if (_isDashPerforming)
                    yield return new WaitForFixedUpdate();

                _currentMovementSpeed = Configuration.BaseSprintSpeed;
                yield return new WaitForFixedUpdate();
            }

            _currentMovementSpeed = Configuration.BaseMovementSpeed;
        }

        private void OnSprintKeyUnpressed(SprintKeyEventArgs args)
        {
            if (args.Cancel)
            {
                if (_sprintCoroutine != null)
                {
                    StopCoroutine(_sprintCoroutine);
                }
                _currentMovementSpeed = Configuration.BaseMovementSpeed;
                _staminaComponent.StopStaminaReduce();
            }
        }

        private void PerformDash()
        {
            if (IsDashCooldown)
                return;

            if (_staminaComponent.CurrentStamina < Configuration.DashStaminaReduce)
                return;
            
            _isDashPerforming = true;
            _dashCooldownTimer = Configuration.DashCooldown;
            _staminaComponent.CurrentStamina -= Configuration.DashStaminaReduce;
                
            StartCoroutine(PerformDashRoutine());
        }

        private IEnumerator PerformDashRoutine()
        {
            float dashTimer = 0f;
            float movementSpeedBeforeDash = _currentMovementSpeed;
            _currentMovementSpeed = Configuration.BaseDashSpeed;

            while (dashTimer < Configuration.DashDuration)
            {
                if (_movementDirection == Vector3.zero)
                {
                    _movementDirection = transform.TransformDirection(Vector3.forward);
                }
                _characterController.Move(_movementDirection * _currentMovementSpeed);
            
                yield return new WaitForEndOfFrame();
                dashTimer += Time.deltaTime;
            }
            
            _currentMovementSpeed = movementSpeedBeforeDash;
            _isDashPerforming = false;
        }

        private void MoveCharacter()
        {
            if (_isDashPerforming)
                return;
            
            _movementDirection = Vector3.right * _unitVector.XSpeedRate + Vector3.forward * _unitVector.YSpeedRate;
            Debug.Log(_movementDirection);

            if (_movementDirection != Vector3.zero)
            {
                _characterController.Move(_movementDirection.normalized * _currentMovementSpeed);
            }
            else
            {
                // Something like animation
            }
        }
    }

    public class UnitVector
    {
        public readonly float UnitSpeedLimit;
        
        private int _x;
        private int _y;
        
        public int X => _x;
        public int Y => _y;
        public float XSpeedRate => _x / UnitSpeedLimit;
        public float YSpeedRate => _y / UnitSpeedLimit;

        public UnitVector(int x = 0, int y = 0)
        {
            UnitSpeedLimit = 15.0f;
            _x = x;
            _y = y;
        }
        
        public void UpdateSpeed(Vector3 rawDirection)
        {
            int newX = rawDirection.x == 0 && X != 0 ? (int)(X * -1.0f / Mathf.Abs(X)) : Mathf.RoundToInt(rawDirection.x);
            int newY = rawDirection.z == 0 && Y != 0 ? (int)(Y * -1.0f / Mathf.Abs(Y)) : Mathf.RoundToInt(rawDirection.z);
            Vector2Int directionVector2 = new Vector2Int(newX, newY);
            
            if (Math.Abs(X + directionVector2.x) <= UnitSpeedLimit)
                _x += directionVector2.x;
            if (Math.Abs(Y + directionVector2.y) <= UnitSpeedLimit)
                _y += directionVector2.y;
        }
    }
}