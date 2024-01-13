using UnityEngine;

namespace Player
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int health;
        public int Health => health;

        public void ApplyDamage(int damage)
        {
            health -= damage;
        }
    }
}