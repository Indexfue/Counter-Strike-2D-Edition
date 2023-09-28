using System;
using System.Collections;
using UnityEngine;

namespace Player.Effects
{
    [RequireComponent(typeof(FieldOfView))]
    public class BlindEffect : Effect
    {
        private float _blindRate;
        private float _oldViewRadius;
        private FieldOfView _targetFOV;

        private IEnumerator PerformEffectRoutine()
        {
            float newViewRadius = _targetFOV.ViewRadius * Mathf.Abs(1 - _blindRate);

            while (true)
            {
                _targetFOV.ViewRadius = newViewRadius;
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDestroy()
        {
            _targetFOV.ViewRadius = _oldViewRadius;
        }

        protected override void PerformEffect() => StartCoroutine(PerformEffectRoutine());

        public void Initialize(float blindRate = 1)
        {
            _targetFOV = GetComponent<FieldOfView>();
            _oldViewRadius = _targetFOV.ViewRadius;
            _blindRate = blindRate;
            PerformEffect();
        }
    }
}