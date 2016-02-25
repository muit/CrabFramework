
namespace Crab {
    using UnityEngine;
    using System;
    using Crab.Components;
    using Crab.Controllers;

    public class Entity : MonoBehaviour
    {
        public bool controlledByFaction = false;

        void Awake()
        {
            controller = GetComponent<EntityController>();

            //Generate Controller
            if (controlledByFaction)
            {
                controller = FactionDatabase.AddController(this, Attributes.faction);
            }
            if (!controller) {
                controller = gameObject.AddComponent<EntityController>();
            }

            AI = controller as AIController;
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
                //controller.AnyDamage(damage, damageCauser, damageType);

                if (IsAI() && !AI.IsInCombatWith(damageCauser))
                {
                    AI.StartCombatWith(damageCauser);
                }
            }
        }

        public bool IsPlayer() {
            return controller as PlayerController;
        }

        private EntityController controller;
        public EntityController Controller {
            get { return controller; }
        }
        [System.NonSerialized]
        public AIController AI;
        public bool IsAI() {
            return AI? AI : AI = controller as AIController;
        }



        //Components

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

        public bool IsEnemyOf(Entity entity) {
            if (!Attributes || !entity.Attributes) return true;
            if (Attributes.faction != entity.Attributes.faction) return true;
            if (Attributes.faction == FactionDatabase.NO_FACTION && 
                entity.Attributes.faction == FactionDatabase.NO_FACTION) return true;
            return false;
        }
    }
}
