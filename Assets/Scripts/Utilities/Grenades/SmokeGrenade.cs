using UnityEngine;
using Utilities.Grenades.GrenadeFields;

namespace Utilities.Grenades
{
    public class SmokeGrenade : Grenade
    {
        [SerializeField] private Smoke _smokePrefab;

        protected override void Explode()
        {
            Instantiate(_smokePrefab, transform.position, Quaternion.identity);
        }
    }
}