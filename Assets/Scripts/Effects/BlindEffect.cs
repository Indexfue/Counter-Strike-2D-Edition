using System.Collections;
using UnityEngine;

namespace Player.Effects
{
    public class BlindEffect : Effect
    {
        private FieldOfView _targetFOV;

        private float _blindRate = 1f;
        private float _blindTime = 3f;
        
        private void Start()
        {
            if (TryGetComponent<FieldOfView>(out _targetFOV))
            {
                PerformEffect();
            }
        }

        protected override IEnumerator PerformEffectRoutine()
        {
            var oldViewRadius = _targetFOV.ViewRadius;
            var oldViewAngle = _targetFOV.ViewAngle;

            _targetFOV.ViewAngle *= Mathf.Abs(1 - _blindRate);
            _targetFOV.ViewRadius *= Mathf.Abs(1 - _blindRate);

            var tick = _blindTime / 100f;
            var oldViewRadiusPerTick = (oldViewRadius - _targetFOV.ViewRadius) / 100f;
            var oldViewAnglePerTick = (oldViewAngle - _targetFOV.ViewAngle) / 100f;

            while (_targetFOV.ViewAngle < oldViewAngle && _targetFOV.ViewRadius < oldViewRadius)
            {
                _targetFOV.ViewAngle += oldViewAnglePerTick;
                _targetFOV.ViewRadius += oldViewRadiusPerTick;
                yield return new WaitForSeconds(tick);
            }
            
            Destroy(this);
        }

        protected override void PerformEffect() => StartCoroutine(PerformEffectRoutine());

        public void Initialize(float blindRate, float blindTime)
        {
            _blindRate = blindRate;
            _blindTime = blindTime;
        }
    }
}