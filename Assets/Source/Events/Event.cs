namespace Crab
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;
    using Crab.Events;

    public class Event : MonoBehaviour
    {
        protected bool started;

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
            started = true;
            JustStarted();
        }

        public void FinishEvent()
        {
            started = false;
            JustFinished();
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
