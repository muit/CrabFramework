namespace Crab
{
    using UnityEngine;
    using System.Collections;
    using Crab.Events;

    public class Event : MonoBehaviour
    {

        protected bool started;

        void Awake()
        {
        }

        public void StartEvent() {
            started = true;
            JustStarted();
        }

        protected virtual void JustStarted() {
            UnityEngine.Debug.Log("Event Started");
        }
    }
}
