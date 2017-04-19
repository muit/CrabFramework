using UnityEngine;
using Crab.Entities;

namespace Crab
{
    [RequireComponent(typeof(CMovement))]
    public class PlayerController : EntityController
    {
        private CMovement movement;

        protected virtual void Awake()
        {
            me = GetComponent<Entity>();
            movement = me.Movement;
        }

        protected virtual void Update()
        {
            movement.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}