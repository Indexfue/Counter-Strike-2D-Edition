using UnityEngine;
using Utilities.Grenades.GrenadeFields;

namespace Utilities.Grenades
{
    public class FireGrenade : Grenade
    {
        [SerializeField] private Fire _firePrefab;

        protected override void Explode()
        {
            Instantiate(_firePrefab, transform.position, Quaternion.identity);
        }
    }
}