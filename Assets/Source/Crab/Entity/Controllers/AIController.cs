using UnityEngine;
using System.Collections.Generic;
using Crab.Entities;

namespace Crab
{

    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(CMovement))]
    [DisallowMultipleComponent]
    public class AIController : EntityController
    {
        enum Events {
            BASIC_ATTACK = 20122
        }


        private CMovement movement;

        void Update() {
            events.Update();
        }

        void JustSpawned()
        {
            movement = GetComponent<CMovement>();
        }

        void EnterCombat(Entity enemy)
        {
            movement.AIMove(enemy.transform);

            events.RegistryEvent((int)Events.BASIC_ATTACK, Random.Range(3000, 6000));
        }

        void FinishCombat(Entity enemy)
        {
            movement.CancelMovement();

            events.CancelEvent((int)Events.BASIC_ATTACK);
        }

        void EnemyLost(Entity enemy)
        {
            movement.AIMove(enemy.transform.position);
        }
        
        public override void AnyDamage(int damage, Entity damageCauser, DamageType damageType) {
        }

        void OnEvent(int id)
        {
            switch ((Events)id)
            {
                case Events.BASIC_ATTACK:
                    events.RestartEvent((int)Events.BASIC_ATTACK, Random.Range(3000, 6000));
                    break;
            }
        }


        //Public Methods
        private HashSet<Entity> targets = new HashSet<Entity>();
        public void StartCombatWith(Entity entity) {
            if (!me.IsEnemyOf(entity) || IsInCombat())
                return;

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