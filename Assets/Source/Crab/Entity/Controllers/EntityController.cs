using UnityEngine;
using Crab.Utils;


namespace Crab
{
    [RequireComponent(typeof(Entity))]
    [DisallowMultipleComponent]
    public class EntityController : MonoBehaviour {
        protected Entity me;
        protected EventsMap<int> events;

        void Start() {
            me = GetComponent<Entity>();
            events = new EventsMap<int>(this);
            SendMessage("JustSpawned");
        }

        //Reusable Methods
        void JustSpawned() { }
        void EnterCombat(Entity target) { }
        void FinishCombat(Entity enemy) { }
        void Update() { }
        void JustDead(Entity killer) { }
        void JustKilled(Entity victim) { }
        protected virtual void AnyDamage(int damage, Entity damageCauser, DamageType damageType) { }
    }
}