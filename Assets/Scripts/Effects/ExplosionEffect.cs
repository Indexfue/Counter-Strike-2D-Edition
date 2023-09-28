namespace Player.Effects
{
    public class ExplosionEffect : Effect
    {
        private int _damage;
        
        protected override void PerformEffect()
        {
            if (gameObject.TryGetComponent<HealthComponent>(out HealthComponent health))
            {
                health.ApplyDamage(_damage);
            }
            
            Destroy(this);
        }

        public void Initialize(int damage)
        {
            _damage = damage;
            PerformEffect();
        }
    }
}