using Interfaces;
using Player;
using UnityEngine;

namespace Weapons.Shooting
{
    public class RaycastShooting : IShooting
    {
        private RaycastHit[] _hittedObjects = new RaycastHit[5];
        
        public void Shoot(Transform shootPoint, WeaponSettings settings, int continiousShotCount, GameObject playerInstance)
        {
            Vector3 lookingDirection = shootPoint.TransformDirection(Vector3.forward) * 50;
            Vector3 bulletDirection = settings.WeaponBallistics.GetBulletDirection(lookingDirection, continiousShotCount, playerInstance);
            var hit = Physics.RaycastNonAlloc(shootPoint.position, bulletDirection, _hittedObjects, settings.ShootDistance, ~settings.IgnoreMask);

            for (int i = 0; i < hit; i++)
            {
                if (_hittedObjects[i].collider.gameObject.TryGetComponent<HealthComponent>(out HealthComponent health))
                {
                    health.ApplyDamage(settings.Damage);
                }
            }
            
            // debug
            RaycastHit debugHit;
            if (Physics.Raycast(shootPoint.position, bulletDirection, out debugHit, 50f))
            {
                Debug.DrawLine(shootPoint.position, debugHit.point, Color.red, 1f);
            }
        }
    }
}