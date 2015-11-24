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
        }


        private EntityController controller;
        public EntityController Controller {
            get { return controller; }
        }

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
