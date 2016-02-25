namespace Crab
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;
    using Crab.Events;

    public class Event : MonoBehaviour
    {
        protected bool started = false;
        protected bool enabled = true;


        public bool disableWhenDone = false;

        public UnityEvent startEvent;
        public UnityEvent finishEvent;

        void Start()
        {
            OnGameStart(SceneScript.Instance);
        }

        public virtual void Reset()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }
        
        public void StartEvent() {
            if (!enabled || started)
                return;

            started = true;
            JustStarted();

            if (disableWhenDone)
            {
                enabled = false;
            }
        }

        public void FinishEvent()
        {
            if (!started) return;
            
            started = false;
            JustFinished();
        }

        public bool IsStarted() {
            return started;
        }

        //Events
        protected virtual void OnGameStart(SceneScript scene) { }

        protected virtual void JustStarted() {
            UnityEngine.Debug.Log("Event Started");
            startEvent.Invoke();
        }

        protected virtual void JustFinished()
        {
            UnityEngine.Debug.Log("Event Finished");
            finishEvent.Invoke();
        }
    }
}
