
namespace Crab {
    using UnityEngine;
    using System;
    using Entities;

    public class Entity : MonoBehaviour
    {
        public bool controlledByFaction = false;

        void Awake()
        {
            m_controller = GetComponent<EntityController>();

            //Generate Controller
            if (controlledByFaction)
            {
                m_controller = FactionDatabase.AddController(this, Attributes.faction);
            }
            if (!m_controller) {
                m_controller = gameObject.AddComponent<EntityController>();
            }
        }

        //Public Methods
        void Damage(int damage, Entity damageCauser, DamageType damageType) {
            Attributes.Live -= damage;

            if (!Attributes.IsAlive())
            {
                m_controller.SendMessage("JustDead", damageCauser);
                damageCauser.controller.SendMessage("JustKilled", this);
            }
            else
            {
                //controller.AnyDamage(damage, damageCauser, damageType);

                if (IsAI() && !ai.IsInCombatWith(damageCauser))
                {
                    ai.StartCombatWith(damageCauser);
                }
            }
        }

        public bool IsPlayer() {
            return m_controller as PlayerController;
        }

        [SerializeField]
        private EntityController m_controller;
        public EntityController controller {
            get { return m_controller; }
        }

        private AIController m_ai;
        public AIController ai {
            get { return m_ai ? m_ai : m_ai = m_controller as AIController; }
        }
        public bool IsAI() {
            return ai;
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
