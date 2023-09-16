using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public struct Ticker
    {
        [SerializeField] private float _tickPerSecond;

        public float TickPerSecond => 1f / _tickPerSecond;
    }
}