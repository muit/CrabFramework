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
            foreach (KeyValuePair<int, Delay> ev in events)
            {
                if (ev.Value.Over())
                {
                    dad.SendMessage("OnEvent", ev.Key);
                    ev.Value.Reset();
                }
            }

        }
    }
}
