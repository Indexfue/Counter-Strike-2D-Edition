using System.Collections;
using UnityEngine;

namespace Player.Effects
{
    public abstract class Effect : MonoBehaviour
    {
        protected abstract IEnumerator PerformEffectRoutine();
        protected abstract void PerformEffect();
    }
}