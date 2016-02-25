using UnityEngine;
using System.Collections;
using Crab;
using Crab.Components;


namespace Crab.Controllers
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