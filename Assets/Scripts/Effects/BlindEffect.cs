using System.Collections;
using UnityEngine;

namespace Player.Effects
{
    [RequireComponent(typeof(FieldOfView))]
    public class BlindEffect : Effect
    {
        private float _blindRate = 1f;
        private float _blindTime = 3f;
        private bool _isInfinityBlind = false;
        
        private FieldOfView _targetFOV;

        private IEnumerator PerformEffectRoutine()
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
                while (_isInfinityBlind)
                    yield return new WaitForEndOfFrame();
                
                _targetFOV.ViewAngle += oldViewAnglePerTick;
                _targetFOV.ViewRadius += oldViewRadiusPerTick;
                yield return new WaitForSeconds(tick);
            }
            
            Destroy(this);
        }

        protected override void PerformEffect() => StartCoroutine(PerformEffectRoutine());

        public void Initialize(float blindRate, float blindTime, bool isInfinityBlind = false)
        {
            _blindRate = blindRate;
            _blindTime = blindTime;
            _isInfinityBlind = isInfinityBlind;
            PerformEffect();
        }
    }
}