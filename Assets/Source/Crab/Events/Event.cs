namespace Crab
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Linq;

    public class Event : MonoBehaviour
    {
        protected bool started = false;
        new protected bool enabled = true;


        public bool disableWhenDone = false;

        public UnityEvent Activation;

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

        public bool IsStarted() {
            return started;
        }


        //Render Event Conections
        void OnDrawGizmos()
        {
            RenderConnection(this, Activation);
        }

        //Events
        protected virtual void OnGameStart(SceneScript scene) { }

        protected virtual void JustStarted() {
            UnityEngine.Debug.Log("Event Started");
            Activation.Invoke();
        }

        public static void RenderConnection(MonoBehaviour caster, UnityEvent ev)
        {
            if (ev == null)
                return;

            Color gizmosColor = Gizmos.color;
            for (int i = 0, len = ev.GetPersistentEventCount(); i < len; i++)
            {
                Event target = ev.GetPersistentTarget(i) as Event;
                if (target)
                {
                    string method = ev.GetPersistentMethodName(i);
                    if (method == "StartEvent")
                        Gizmos.color = Color.green;
                    else if (method == "set_enabled")
                        Gizmos.color = Color.blue;
                    else
                        return;

                    Gizmos.DrawLine(caster.transform.position, target.transform.position);
                }
            }
            Gizmos.color = gizmosColor;
        }
    }
}
