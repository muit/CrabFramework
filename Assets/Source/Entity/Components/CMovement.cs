using UnityEngine;
using System.Collections;
using Crab;
using Crab.Utils;

namespace Crab.Components
{
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public class CMovement : MonoBehaviour
    {
        private Entity me;
        private CharacterController characterController;
        private Animator animator;
        private EntityFloor floor;

        //Pathfinding
        public float reachDistance = 100;
        private NavMeshAgent agent;
        private Transform agentTarget;

        void Awake()
        {
            me = GetComponent<Entity>();
            floor = GetComponentInChildren<EntityFloor>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();

            agent = GetComponent<NavMeshAgent>();
        }


        public void Move(Vector3 direction)
        {
            animator.SetFloat("Speed", direction.z);
            animator.SetFloat("Direction", direction.x);
        }

        public void AIMove(Vector3 position, float reachDistance = 1)
        {
            if (!agent) return;
            agentTarget = null;
            agent.SetDestination(position);
        }

        public void AIMove(Transform trans, float reachDistance = 1)
        {
            if (!agent) return;
            agentTarget = trans;
        }

        void Update()
        {
            if (agent)
            {
                if (agent.remainingDistance < reachDistance)
                {
                    agent.Stop();
                }
                else if (agentTarget)
                {
                    agent.SetDestination(agentTarget.position);
                }
            }
        }

        /**
         * Crouch
         */
        private bool crouching;
        public bool canCrouch = true;

        public void StartCrouching()
        {
            if (canCrouch)
            {
                crouching = true;
                animator.SetBool("Crouch", true);
            }
        }
        public void StopCrouching()
        {
            crouching = false;
            animator.SetBool("Crouch", false);
        }

        public bool isCrouching
        {
            get
            {
                return crouching;
            }
        }


        /**
         * Sprint
         */
        private float sprinting;
        public bool canSprint = true;

        public void StartSprint()
        {
            if (canSprint)
            {
                sprinting = 1f;
                animator.SetFloat("Sprint", sprinting);
            }
        }

        public void StopSprint()
        {
            sprinting = 0f;
            animator.SetFloat("Sprint", sprinting);
        }

        public bool isSprinting
        {
            get
            {
                return sprinting > 0;
            }
        }

        /**
         * Physics
         */
        private bool falling;

        public void StartFalling()
        {
            if (!falling)
            {
                animator.SetTrigger("StartFalling");
            }
            falling = true;
            animator.SetBool("Falling", falling);
        }

        public void StopFalling()
        {
            falling = false;
            animator.SetBool("Falling", falling);
        }

        public bool isFalling
        {
            get
            {
                return falling;
            }
        }
    }
}