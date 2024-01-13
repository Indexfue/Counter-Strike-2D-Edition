using System;
using UnityEngine;

namespace Weapons.Recoil
{
    [Serializable]
    public sealed class RecoilPattern
    {
        [SerializeField] private RecoilPatternType recoilPatternType;
        
        public float[] GetRecoilPattern()
        {
            switch (recoilPatternType)
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
            0f, 0.5f, 1f, 2f, 2.5f, 3f, 2.5f, 
            2f, 1.5f, 1.8f, 2f, 1.8f, 1.5f, 
            -2f, -1.5f, -1f, -0.5f, -1f,
            -1.5f, -2f, -1.5f, -1.2f, -0.5f, 0f, 
            0.5f, 0.8f, 1.5f, 1.2f, 0.5f, 0.2f
        };

        public readonly float[] RecoilPattern_Deagle = new[]
        {
            0f, 3f, -5f, 2f, -3f, 4f, -1f
        };
    }
}