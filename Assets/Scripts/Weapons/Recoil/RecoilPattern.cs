using System;
using UnityEngine;

namespace Weapons.Recoil
{
    [Serializable]
    public sealed class RecoilPattern
    {
        [SerializeField] private RecoilPatternType _recoilPatternType;
        
        public float[] GetRecoilPattern()
        {
            switch (_recoilPatternType)
            {
               case RecoilPatternType.AK47:
                   return RecoilPattern_AK47;
               case RecoilPatternType.Deagle:
                   return RecoilPattern_Deagle;
            }

            return null;
        }
        
        public readonly float[] RecoilPattern_AK47 = new[]
        {
            0f, 0f, 0f, 2f, 2f, 2f, 2f, 
            1f, 1f, 1f, 1f, 1f, 1f, 
            -2f, -2f, -2f, -2f, -2f,
            -2f, -2f, 2f, 2f, 2f, -3f, 
            -2f, -1f, -2f, -3f, 2f, 2.5f
        };

        public readonly float[] RecoilPattern_Deagle = new[]
        {
            0f, 3f, -5f, 2f, -3f, 4f, -1f
        };
    }
}