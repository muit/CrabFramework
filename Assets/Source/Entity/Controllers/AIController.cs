using UnityEngine;
using System.Collections;
using Crab;
using Crab.Components;


namespace Crab.Controllers
{
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(CMovement))]
    [DisallowMultipleComponent]
    public class AIController : EntityController
    {
        private Entity me;
        private CMovement movement;

        void Awake()
        {
            me = GetComponent<Entity>();
            movement = GetComponent<CMovement>();
        }

        public void EnemyDetected(Entity enemy)
        {
            movement.AIMove(enemy.transform);
        }

        public void EnemyLost(Entity enemy)
        {
            movement.AIMove(enemy.transform.position);
        }
    }
}