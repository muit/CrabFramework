using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Crab.Utils
{
    public class EventsMap
    {
        private MonoBehaviour dad;
        private Dictionary<int, Delay> events = new Dictionary<int, Delay>();


        public EventsMap(MonoBehaviour dad) {
            this.dad = dad;
            if (!dad) throw new System.Exception("EventsMap dad can't be null or undefined.");
        }

        public void RegistryEvent(int id, int duration)
        {
            events[id] = new Delay(duration, true);
        }

        public void RestartEvent(int id, int duration)
        {
            RegistryEvent(id, duration);
        }

        public void CancelEvent(int id) {
            events.Remove(id);
        } 

        public void Update()
        {
            List<KeyValuePair<int, Delay>> doneEvents = events.Where(ev => { return ev.Value.Over(); }).ToList();
            foreach (KeyValuePair<int, Delay> ev in doneEvents)
            {
                ev.Value.Reset();
                dad.SendMessage("OnEvent", ev.Key);
                CancelEvent(ev.Key);
            }
        }
    }
}
