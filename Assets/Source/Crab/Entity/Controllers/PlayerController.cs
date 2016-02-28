using UnityEngine;
using Crab.Entities;

namespace Crab
{
    [RequireComponent(typeof(CMovement))]
    public class PlayerController : EntityController
    {
        private CMovement movement;
        void Awake()
        {
            me = GetComponent<Entity>();
            movement = me.Movement;
        }

        void Update()
        {
            movement.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}