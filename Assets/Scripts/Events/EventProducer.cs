using System;
using System.Collections.Generic;

namespace Events
{
    public class EventProducer<T>
    {
        private readonly List<Action<T>> listeners = new List<Action<T>>();

        public void AddListener(Action<T> handler)
        {
            listeners.Add(handler);
        }

        public void RemoveListener(Action<T> handler)
        {
            listeners.Remove(handler);
        }

        public void Handle(T e)
        {
            foreach (var listener in listeners)
            {
                listener(e);
            }
        }
        
    }
}