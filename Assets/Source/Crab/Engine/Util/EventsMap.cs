using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Crab.Utils
{
    //public class EventsMap : EventsMap<int> {}

    public class EventsMap<T>
        where T : IConvertible, IComparable, IFormattable
    {
        protected MonoBehaviour dad;
        protected Dictionary<T, Delay> events = new Dictionary<T, Delay>();

        public EventsMap() { }
        public EventsMap(MonoBehaviour dad) {
            this.dad = dad;
            if (!dad) throw new System.Exception("EventsMap dad can't be null or undefined.");
        }

        
        public void RegistryEvent<E>(E id, int duration)
            where E : struct, IConvertible, IComparable, IFormattable
        {
            if (typeof(E).IsEnum)
            {
                RegistryEvent(Convert.ToInt32(id), duration);
            }
        }

        public void RegistryEvent(T id, int duration)
        {
            events[id] = new Delay(duration, true);
        }

        public void RestartEvent(T id, int duration)
        {
            RegistryEvent(id, duration);
        }

        public void CancelEvent(T id) {
            events.Remove(id);
        } 

        public void Update()
        {
            List<KeyValuePair<T, Delay>> doneEvents = events.Where(ev => { return ev.Value.Over(); }).ToList();
            foreach (KeyValuePair<T, Delay> ev in doneEvents)
            {
                ev.Value.Reset();
                CancelEvent(ev.Key);
                if (dad)
                    dad.SendMessage("OnEvent", ev.Key);
            }
        }
    }
}
