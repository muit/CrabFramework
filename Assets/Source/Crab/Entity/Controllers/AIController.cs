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

        public int damage = 5;
        public float attackDistance = 2.0f;
        public int speedLosePctOnDamage = 10;

        [Header("Despawn")]
        public bool despawnAfterDead = true;
        public float despawnDelay = 2.0f;

        private CMovement Movement {
            get { return Me.Movement; }
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void JustSpawned()
        {
        }

        protected override void EnterCombat(Entity target)
        {
            Movement.AIMove(target.transform);

            events.RegistryEvent((int)Events.BASIC_ATTACK, Random.Range(1000, 2000));
        }

        protected override void FinishCombat(Entity enemy)
        {
            Movement.CancelMovement();

            events.CancelEvent((int)Events.BASIC_ATTACK);
        }
        
        protected override void JustDied(Entity killer) {
            StopCombat();

            Destroy(gameObject, despawnDelay);
        }


        void EnemyLost(Entity enemy)
        {
            Movement.AIMove(enemy.transform.position);
        }
        
        public override void AnyDamage(int damage, Entity damageCauser, DamageType damageType) {
            base.AnyDamage(damage, damageCauser, damageType);

            Movement.ReduceSpeedByPct(speedLosePctOnDamage);
        }

        void OnEvent(int id)
        {
            switch ((Events)id)
            {
                case Events.BASIC_ATTACK:
                    //Search a close target
                    foreach(Entity target in targets) {
                        if (me.DistanceTo(target) < attackDistance)
                        {
                            target.Damage(damage, me);

                            //Only attack to 1 target
                            break;
                        }
                    }

                    events.RestartEvent((int)Events.BASIC_ATTACK, Random.Range(1000, 1500));
                    break;
            }
        }


        //Public Methods
        private HashSet<Entity> targets = new HashSet<Entity>();
        public void StartCombatWith(Entity entity) {
            if (!Me.IsAlive())
                return;

            if (!Me.IsEnemyOf(entity) || IsInCombat())
                return;

            targets.Add(entity);
            SendMessage("EnterCombat", entity);
        }

        public void StopCombatWith(Entity entity)
        {
            targets.Remove(entity);
            if (targets.Count == 0)
            {
                FinishCombat(entity);
            }
        }

        public void StopCombat()
        {
            targets.Clear();
            FinishCombat(null);
        }

        public bool IsInCombat() {
            return targets.Count > 0;
        }
        public bool IsInCombatWith(Entity entity) {
            return targets.Contains(entity);
        }
    }
}