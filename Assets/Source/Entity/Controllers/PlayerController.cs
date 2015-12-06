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
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            movement.Move(h, v);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                movement.StartSprint();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                movement.StopSprint();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                movement.StartCrouching();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                movement.StopCrouching();
            }
        }
    }
}