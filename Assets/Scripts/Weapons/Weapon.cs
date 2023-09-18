using System.Collections;
using Interfaces;
using Player;
using Weapons.Shooting;
using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour, IInventoryItem
    {
        public WeaponSettings Settings;
        
        [SerializeField] private int _currentCapacity;

        private IShooting _shootingLogic;
        private float _lastShotTime;
        private int _continiousShotCount = 0;

        private Coroutine _attackingRoutine;
        private Coroutine _reloadingRoutine;
        private Coroutine _coolingRoutine;

        private void Start()
        {
            Settings.Initialize(gameObject, transform);
            InitializeShootingLogic();
        }

        private void OnFireKeyPressed(FireKeyPressedEventArgs args) => StartRoutineAsVariable(AttackingRoutine(), ref _attackingRoutine);

        private void OnReloadKeyPressed(ReloadKeyPressedEventArgs args) => StartRoutineAsVariable(ReloadRoutine(), ref _reloadingRoutine);

        private void OnFireKeyUnpressed(FireKeyUnpressedEventArgs args)
        {
            StopRoutineAsVariable(ref _attackingRoutine);
            StartRoutineAsVariable(WeaponCoolingRoutine(), ref _coolingRoutine);
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
            _shootingLogic.Shoot(Settings.ShootPoint, Settings, _continiousShotCount);
            _currentCapacity -= 1;
            _continiousShotCount = Mathf.Clamp(_continiousShotCount + 1, 0, Settings.MaxCapacity);
        }
        
        private bool TryAttack()
        {
            if (Settings.WeaponType != WeaponType.Melee && _currentCapacity == 0) return false;

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
            _currentCapacity = Settings.MaxCapacity;
            StopRoutineAsVariable(ref _reloadingRoutine);
        }

        private IEnumerator AttackingRoutine()
        {
            if (_coolingRoutine != null)
                StopRoutineAsVariable(ref _coolingRoutine);

            if (!Settings.CanClampShooting && !IsShootingDelayed())
                TryAttack();
            
            if (Settings.CanClampShooting)
            {
                while (!IsShootingDelayed())
                    yield return new WaitForEndOfFrame();
                
                while (true)
                {
                    if (TryAttack() == false) 
                        break;
                    yield return new WaitForSeconds(Settings.Ticker.TickPerSecond);
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
        private void OnDrawGizmos()
        {
            switch (Settings.WeaponCastType)
            {
                case WeaponCastType.None:
                    break;
                case WeaponCastType.Raycast:
                    DrawLine();
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

            if (Physics.Raycast(transform.position, direction, out hit, 50f))
            {
                Gizmos.DrawLine(transform.position, hit.point);
            }
            else
            {
                Gizmos.DrawRay(transform.position, direction);
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