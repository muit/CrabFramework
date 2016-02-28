using UnityEngine;
using System.Collections;
using Crab;


namespace Crab.Entities
{
    [RequireComponent(typeof(Entity))]
    [DisallowMultipleComponent]
    public class CState : MonoBehaviour
    {
        public enum CombatState
        {
            Idle,
            Alert,
            Combat
        }

        private Entity me;
        void Awake()
        {
            me = GetComponent<Entity>();

            combat = CombatState.Idle;
        }

        public CombatState combat;
    }
}