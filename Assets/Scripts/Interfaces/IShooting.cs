using Weapons;
using UnityEngine;

namespace Interfaces
{
    public interface IShooting
    {
        public void Shoot(Transform shootPoint, WeaponSettings settings, int continiousShotCount, GameObject playerInstance);
    }
}