using UnityEngine;
using Crab.Components;
using Crab.Controllers;

namespace Crab
{
    public class Entity : MonoBehaviour
    {
        public bool controlledByFaction = false;

        void Awake() {
            controller = GetComponent<EntityController>();
            AI = (AIController)controller;
        }

        //Public Methods
        void Damage(int damage, Entity damageCauser, DamageType damageType) {
            Attributes.Live -= damage;

            if (!Attributes.IsAlive())
            {
                controller.SendMessage("JustDead", damageCauser);
                damageCauser.Controller.SendMessage("JustKilled", this);
            }
            else
            {
                controller.AnyDamage(damage, damageCauser, damageType);

                if (IsAI() && !AI.IsInCombatWith(damageCauser))
                {
                    AI.StartCombatWith(damageCauser);
                }
            }
        }


        //Components
        private EntityController controller;
        public EntityController Controller {
            get { return controller; }
        }
        public AIController AI;
        public bool IsAI() { return AI; }

        private CMovement movement;
        public CMovement Movement {
            get {
                return movement ? movement : movement = GetComponent<CMovement>();
            }
        }

        private CState state;
        public CState State {
            get {
                return state ? state : state = GetComponent<CState>();
            }
        }

        private CAttributes attributes;
        public CAttributes Attributes {
            get {
                return attributes ? attributes : attributes = GetComponent<CAttributes>();
            }
        }

        private CInventory inventory;
        public CInventory Inventory {
            get {
                return inventory? inventory : inventory = GetComponent<CInventory>();
            }
        }
    }
}
