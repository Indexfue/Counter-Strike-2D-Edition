using UnityEngine;
using Utilities.Grenades.GrenadeFields;

namespace Utilities.Grenades
{
    public class SmokeGrenade : Grenade
    {
        [SerializeField] private Smoke smokePrefab;

        protected override void Explode()
        {
            Instantiate(smokePrefab, transform.position, Quaternion.identity);
        }
    }
}