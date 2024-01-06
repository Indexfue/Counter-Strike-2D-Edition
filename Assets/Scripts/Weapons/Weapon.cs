using System;
using System.Collections;
using Interfaces;
using Player;
using Player.GUI;
using Weapons.Shooting;
using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour, IInventoryItem
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private WeaponSettings settings;
        //TODO: Rewrite this below
        [SerializeField] private int currentCapacity;

        private IShooting _shootingLogic;
        private float _lastShotTime;
        private int _continiousShotCount = 0;

        private Coroutine _attackingRoutine;
        private Coroutine _reloadingRoutine;
        private Coroutine _coolingRoutine;

        private CameraShake _cameraShake;
        
        public WeaponSettings Settings
        {
            get => settings;
            set
            {
                if (value.WeaponType != weaponType)
                {
                    throw new ArgumentException("Can't set weapon with different type");
                }

                settings = value;
            }
        }

        private void Start()
        {
            Settings.Initialize(gameObject, transform);
            InitializeShootingLogic();
        }

        protected void OnFireKeyPressed(FireKeyPressedEventArgs args)
        {
            if (gameObject.activeSelf)
                StartRoutineAsVariable(AttackingRoutine(), ref _attackingRoutine);
        } 
        
        protected void OnReloadKeyPressed(ReloadKeyPressedEventArgs args)
        {
            if (gameObject.activeSelf)
                StartRoutineAsVariable(ReloadRoutine(), ref _reloadingRoutine);
        }

        protected void OnFireKeyUnpressed(FireKeyUnpressedEventArgs args)
        {
            if (gameObject.activeSelf)
            {
                StopRoutineAsVariable(ref _attackingRoutine);
                StartRoutineAsVariable(WeaponCoolingRoutine(), ref _coolingRoutine);
            }
        }

        private void InitializeShootingLogic()
        {
            switch (Settings.WeaponCastType)
            {
                case WeaponCastType.Raycast:
                    _shootingLogic = new RaycastShooting();
                    break;
                case WeaponCastType.Overlap:
                    _shootingLogic = new OverlapShooting();
                    break;
                default:
                    return;
            }
        }

        private void PerformAttack()
        {
            GameObject playerInstance = gameObject.GetComponentInParent(typeof(PlayerMovement)).gameObject;
            _shootingLogic.Shoot(Settings.ShootPoint, Settings, _continiousShotCount, playerInstance);

            if (Settings.WeaponType != WeaponType.Melee)
            {
                currentCapacity -= 1;
                _continiousShotCount = Mathf.Clamp(_continiousShotCount + 1, 0, Settings.MaxCapacity);
                _lastShotTime = Time.time;
            }
        }
        
        private bool TryAttack()
        {
            if (Settings.WeaponType != WeaponType.Melee && currentCapacity == 0) return false;

            PerformAttack();
            return true;
        }
        
        private bool IsShootingDelayed() => Time.time - _lastShotTime >= Settings.ShootDelay;

        private void StartRoutineAsVariable(IEnumerator routine, ref Coroutine routineVariable)
        {
            if (routineVariable != null) return;
            routineVariable = StartCoroutine(routine);
        }

        private void StopRoutineAsVariable(ref Coroutine routineVariable)
        {
            if (routineVariable == null) return;
            
            StopCoroutine(routineVariable);
            routineVariable = null;
        }

        private IEnumerator ReloadRoutine()
        {
            Debug.Log("Reloading");
            yield return new WaitForSeconds(Settings.ReloadDuration);
            currentCapacity = Settings.MaxCapacity;
            StopRoutineAsVariable(ref _reloadingRoutine);
        }

        private IEnumerator AttackingRoutine()
        {
            if (_coolingRoutine != null)
                StopRoutineAsVariable(ref _coolingRoutine);

            if (!Settings.CanClampShooting && IsShootingDelayed())
            {
                TryAttack();
            }

            if (Settings.CanClampShooting)
            {
                while (!IsShootingDelayed())
                    yield return new WaitForEndOfFrame();
                
                while (true)
                {
                    if (!TryAttack()) 
                        break;
                    yield return new WaitForSeconds(Settings.ShotsPerSecond);
                }
            }
        }

        private IEnumerator WeaponCoolingRoutine()
        {
            int continiousShotCountOld = _continiousShotCount;
            float currentCoolingWeaponDuration = Settings.CoolingWeaponDuration / continiousShotCountOld;
            
            while (_continiousShotCount != 0)
            {
                yield return new WaitForSeconds(currentCoolingWeaponDuration);
                _continiousShotCount -= 1;
            }
        }

        public void Select()
        {
            gameObject.SetActive(true);
        }

        public void Deselect()
        {
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!gameObject.activeInHierarchy)
                return;

            Settings = settings;
        }
        
        private void OnDrawGizmos()
        {
            if (Settings == null) return;
            
            switch (Settings.WeaponCastType)
            {
                case WeaponCastType.None:
                    break;
                case WeaponCastType.Raycast:
                    DrawLine();
                    DrawSpread();
                    break;
                case WeaponCastType.Overlap:
                    DrawSphere();
                    break;
            }
        }

        private void DrawLine()
        {
            Gizmos.color = Color.green;
            Vector3 direction = transform.TransformDirection(Vector3.forward) * 50f;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, Settings.ObstacleMask))
            {
                Gizmos.DrawLine(transform.position, hit.point);
            }
            else
            {
                Gizmos.DrawRay(transform.position, direction);
            }
        }

        private void DrawSpread()
        {
            Gizmos.color = Color.cyan;
            Vector3 direction = transform.TransformDirection(Vector3.forward) * 50f;
            RaycastHit hit;
            
            float spreadRadiusLength = 0.1f;

            if (Settings.WeaponBallistics.UseSpread)
            {
                spreadRadiusLength = settings.WeaponBallistics.SpreadRadius / 2;
                PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();

                if (playerMovement is not null)
                {
                    spreadRadiusLength += playerMovement.CurrentMovementSpeed * Settings.WeaponBallistics.SpreadRadius;
                }
            }
            Vector3 spreadCubeSize = new Vector3(spreadRadiusLength, 0.1f, 0.1f);
            
            if (Physics.Raycast(transform.position, direction, out hit, Settings.ObstacleMask))
            {
                Gizmos.DrawCube(hit.point, spreadCubeSize);
            }
        }

        private void DrawSphere()
        {
            Gizmos.color = Color.green;
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            Gizmos.DrawSphere(new Vector3(transform.position.x + direction.x,transform.position.y +  direction.y,transform.position.z +  direction.z), 1f);
        }
#endif
    }
}