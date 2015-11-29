using Crab.Components;

namespace Crab.Controllers
{
    using UnityEngine;
    using System.Collections.Generic;

    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(CMovement))]
    [DisallowMultipleComponent]
    public class AIController : EntityController
    {
        private CMovement movement;

        void JustSpawned()
        {
            movement = GetComponent<CMovement>();
        }

        void EnterCombat(Entity enemy)
        {
            movement.AIMove(enemy.transform);
        }

        void FinishCombat(Entity enemy)
        {
            movement.CancelMovement();
        }

        void EnemyLost(Entity enemy)
        {
            movement.AIMove(enemy.transform.position);
        }
        
        public override void AnyDamage(int damage, Entity damageCauser, DamageType damageType) {
        }


        //Public Methods
        private HashSet<Entity> targets = new HashSet<Entity>();
        public void StartCombatWith(Entity entity) {
            targets.Add(entity);
            SendMessage("EnterCombat", entity);
        }

        public void StopCombatWith(Entity entity)
        {
            targets.Remove(entity);
            if (targets.Count == 0)
            {
                SendMessage("FinishCombat", entity);
            }
        }
        
        public bool IsInCombat() {
            return targets.Count > 0;
        }
        public bool IsInCombatWith(Entity entity) {
            return targets.Contains(entity);
        }

    }
}