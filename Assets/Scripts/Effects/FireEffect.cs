using System.Collections;
using UnityEngine;

namespace Player.Effects
{
    [RequireComponent(typeof(HealthComponent))]
    public class FireEffect : Effect
    {
        private float _tick = 0.4f;
        private int _damage;

        private HealthComponent _healthComponent;

        private IEnumerator PerformEffectRoutine()
        {
            _healthComponent.ApplyDamage(_damage);
            yield return new WaitForSeconds(_tick);
        }
        
        protected override void PerformEffect()
        {
            StartCoroutine(PerformEffectRoutine());
        }

        public void Initialize(int damage, float tick)
        {
            _damage = damage;
            _tick = tick;
            _healthComponent = GetComponent<HealthComponent>();
            PerformEffect();
        }
    }
}