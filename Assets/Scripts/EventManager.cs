using System;
using System.Collections.Generic;

namespace Player
{
    public class EventManager
    {
        private static Dictionary<Type, List<Action<EventArgs>>> _eventSubscribers = new Dictionary<Type, List<Action<EventArgs>>>();

        public static void Subscribe<T>(Action<T> handler) where T : EventArgs
        {
            Type eventType = typeof(T);
            if (!_eventSubscribers.ContainsKey(eventType))
            {
                _eventSubscribers[eventType] = new List<Action<EventArgs>>();
            }

            _eventSubscribers[eventType].Add(e => handler((T)e));
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : EventArgs
        {
            Type eventType = typeof(T);
            if (_eventSubscribers.ContainsKey(eventType))
            {
                _eventSubscribers[eventType].Remove(e => handler((T)e));
            }
        }

        public static void RaiseEvent<T>(T args) where T : EventArgs
        {
            Type eventType = typeof(T);
            if (_eventSubscribers.ContainsKey(eventType))
            {
                foreach (Action<EventArgs> handler in _eventSubscribers[eventType])
                {
                    handler(args);
                }
            }
        }
    }
}