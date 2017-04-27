using UnityEngine;
using UnityEngine.Events;
using Crab.Utils;


namespace Crab
{
    [RequireComponent(typeof(Entity))]
    [DisallowMultipleComponent]
    public class EntityController : MonoBehaviour {

        public DamageEvent OnAnyDamage;

        protected Entity me;
        public Entity Me {
            get {
                return me? me : me = GetComponent<Entity>();
            }
        }

        protected EventsMap<int> events = new EventsMap<int>();

        void Start() {
            me = GetComponent<Entity>();
            events.Father = this;
            SendMessage("JustSpawned");
        }

        //Reusable Methods
        protected virtual void JustSpawned() { }
        protected virtual void EnterCombat(Entity target) { }
        protected virtual void FinishCombat(Entity enemy) { }
        protected virtual void Update()
        {
            events.Update();
        }
        protected virtual void JustDied(Entity killer) { }
        protected virtual void JustKilled(Entity victim) { }

        public virtual void AnyDamage(int damage, Entity damageCauser, DamageType damageType) {
            OnAnyDamage.Invoke(damage, damageCauser);
        }


        [System.Serializable]
        public class DamageEvent : UnityEvent<int, Entity> { }
    }
}