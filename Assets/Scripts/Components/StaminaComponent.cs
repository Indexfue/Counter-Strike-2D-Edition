using System.Collections;
using Player;
using UnityEngine;

namespace Components
{
    public class StaminaComponent : MonoBehaviour
    {
        [SerializeField] private bool isInfinityStamina;
        [SerializeField] private int staminaLevel;

        private float _currentStamina;
        private Coroutine _coroutine;
        private float _regenerationStartTimer;
        
        public float MaxStaminaByLevel => Configuration.StaminaPerLevel * staminaLevel + Configuration.BaseStaminaValue;
        
        public float CurrentStamina
        {
            get => _currentStamina;
            set
            {
                _currentStamina = Mathf.Clamp(value, 0, MaxStaminaByLevel);

                if (value < _currentStamina)
                {
                    _regenerationStartTimer = 0f;
                }
                Debug.Log($"Current stamina = {_currentStamina}");
            }
        }

        public void StopStaminaReduce() => StopCoroutines();
        
        public void HandleStaminaReduce()
        {
            StopCoroutines();
            _coroutine = StartCoroutine(HandleStaminaReduceCoroutine());
        }

        private void Start()
        {
            CurrentStamina = Configuration.BaseStaminaValue;
        }

        private void Update()
        {
            if (NeedToStaminaRegeneration())
            {
                _coroutine = StartCoroutine(HandleStaminaRegenerationCoroutine());
            }
        }

        private void StopCoroutines()
        {
            if (_coroutine is not null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private bool NeedToStaminaRegeneration()
        {
            if (_coroutine is null && CurrentStamina < MaxStaminaByLevel)
            {
                _regenerationStartTimer += Time.deltaTime;
                if (_regenerationStartTimer > Configuration.StaminaRegerationStartTimer)
                {
                    _regenerationStartTimer = 0f;
                    return true;
                }
            }

            return false;
        }

        private IEnumerator HandleStaminaReduceCoroutine()
        {
            while (CurrentStamina > 0)
            {
                if (isInfinityStamina)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                CurrentStamina -= Configuration.StaminaReducingPerSecond / 10f;
                yield return new WaitForSeconds(0.1f);
            }

            StopCoroutines();
        }

        private IEnumerator HandleStaminaRegenerationCoroutine()
        {
            while (CurrentStamina < MaxStaminaByLevel)
            {
                CurrentStamina += Configuration.StaminaRegenerationPerSecond / 10f;
                yield return new WaitForSeconds(0.1f);
            }

            CurrentStamina = MaxStaminaByLevel;
            StopCoroutines();
        }
    }
}