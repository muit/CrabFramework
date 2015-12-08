
namespace Crab.Components {
    using UnityEngine;
    using Utils;

    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(NavMeshAgent))]

    [DisallowMultipleComponent]
    public class CMovement : MonoBehaviour {
        public float speed = 3.5f;
        public float sideSpeed = 3f;
        public float accelerationForce = 200;
        public float rotateSpeed = 15;

        private Entity me;
        private Animator animator;
        private EntityFloor floor;
        //Camera
        [System.NonSerialized]
        public Vector3 viewDirection;
        
        private Vector3 moveVector;


        //Pathfinding
        [Header("Pathfinding")]
        public float reachDistance = 1;
        private NavMeshAgent agent;
        private Transform agentTarget;
        private CharacterController characterController;

        void Awake() {
            me = GetComponent<Entity>();
            floor = GetComponentInChildren<EntityFloor>();
            animator = GetComponentInChildren<Animator>();

            characterController = GetComponent<CharacterController>();

            agent = GetComponent<NavMeshAgent>();
            if (agent)
            {
                agent.stoppingDistance = reachDistance;
                agent.speed = speed;
                agent.acceleration = accelerationForce;
            }
        }


        public void Move(float h, float v) {
            if (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f)
            {
                moveVector = new Vector3(h * sideSpeed, 0, v*speed);
            }
            else
            {
                moveVector = Vector3.zero;
            }
        }

        public void AIMove(Vector3 position, float reachDistance = 1) {
            if (!agent)
                return;
            agentTarget = null;
            agent.SetDestination(position);
            agent.Resume();
        }

        public void AIMove(Transform trans, float reachDistance = 1) {
            if (!agent)
                return;
             
            agentTarget = trans;
            agent.SetDestination(trans.position);
            agent.Resume();
        }

        public void CancelMovement() {
            if (agent)
            {
                agent.Stop();
                agentTarget = null;
            }
        }

        void Update() {
            if (agent)
            {
                if (agentTarget)
                {
                    agent.destination = agentTarget.position;
                    if (agent.remainingDistance >= reachDistance)
                    {
                        agent.Resume();
                    }
                }
            }
            if (IsMoving())//IsMoving
            {
                if (characterController)
                {
                    characterController.Move(transform.TransformDirection(moveVector) * Time.deltaTime);
                }
                else if (agent)
                {
                    agent.Move(transform.TransformDirection(moveVector) * Time.deltaTime);
                }

                //Rotate
                if (viewDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(viewDirection), rotateSpeed * Time.deltaTime);
                }
            }
        }

        public bool IsMoving() {
            return moveVector != Vector3.zero;
        }

        private bool TargetIsUpdated() {
            return agent.destination == agentTarget.position;
        }

        /**
         * Crouch
         */
        private bool crouching;
        public bool canCrouch = true;

        public void StartCrouching() {
            if (canCrouch)
            {
                crouching = true;
                animator.SetBool("Crouch", true);
            }
        }
        public void StopCrouching() {
            crouching = false;
            animator.SetBool("Crouch", false);
        }

        public bool isCrouching {
            get {
                return crouching;
            }
        }


        /**
         * Sprint
         */
        private float sprinting;
        public bool canSprint = true;

        public void StartSprint() {
            if (canSprint)
            {
                sprinting = 1f;
                animator.SetFloat("Sprint", sprinting);
            }
        }

        public void StopSprint() {
            sprinting = 0f;
            animator.SetFloat("Sprint", sprinting);
        }

        public bool isSprinting {
            get {
                return sprinting > 0;
            }
        }

        /**
         * Physics
         */
        private bool falling;

        public void StartFalling() {
            if (!falling)
            {
                animator.SetTrigger("StartFalling");
            }
            falling = true;
            animator.SetBool("Falling", falling);
        }

        public void StopFalling() {
            falling = false;
            animator.SetBool("Falling", falling);
        }

        public bool isFalling {
            get {
                return falling;
            }
        }
    }
}