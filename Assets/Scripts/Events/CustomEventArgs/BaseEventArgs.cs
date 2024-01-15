using System;
using UnityEngine;

namespace Player
{
    public abstract class BaseEventArgs : EventArgs
    {
        public GameObject Sender { get; }

        protected BaseEventArgs(GameObject sender) => Sender = sender;
    }
}