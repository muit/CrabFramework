
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
                string faction = Enum.GetName(typeof(Faction), Attributes.faction);
                UnityEngine.Debug.Log("Loading Faction controller \"" + faction + "Controller\"");
                try
                {
                    Type type = Type.GetType(faction+"Controller");
                    controller = gameObject.AddComponent(type) as FactionController;
                }
                catch (Exception) {
                    UnityEngine.Debug.LogError("Couldn't load \"" + faction + "Controller\"");
                }
                //Attributes.faction;
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
        [System.NonSerialized]
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
