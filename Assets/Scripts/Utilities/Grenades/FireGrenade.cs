using UnityEngine;
using Utilities.Grenades.GrenadeFields;

namespace Utilities.Grenades
{
    public class FireGrenade : Grenade
    {
        [SerializeField] private Fire firePrefab;

        protected override void Explode()
        {
            Instantiate(firePrefab, transform.position, Quaternion.identity);
        }
    }
}