using UnityEngine;

namespace Player
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        public int Health => _health;

        public void ApplyDamage(int damage)
        {
            _health -= damage;
        }
    }
}