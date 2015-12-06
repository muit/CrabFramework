
namespace Crab.Components {
    using UnityEngine;
    using Utils;

    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(NavMeshAgent))]

    [DisallowMultipleComponent]
    public class CMovement : MonoBehaviour {
        private Entity me;
        new private Rigidbody rigidbody;
        private Animator animator;
        private EntityFloor floor;
        [System.NonSerialized]
        public Vector3 cachedDirection;

        public float speed = 3.5f;
        public float accelerationForce = 200;
        public float rotateSpeed = 15;

        //Pathfinding
        [Header("Pathfinding")]
        public float reachDistance = 1;
        private NavMeshAgent agent;
        private Transform agentTarget;

        void Awake() {
            me = GetComponent<Entity>();
            floor = GetComponentInChildren<EntityFloor>();
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();

            agent = GetComponent<NavMeshAgent>();
            if (agent)
            {
                agent.stoppingDistance = reachDistance;
                agent.speed = speed;
                agent.acceleration = accelerationForce;
            }
        }


        public void Move(float h, float v) {
            rigidbody.AddRelativeForce(new Vector3(h, 0, v) * accelerationForce);

            Rotate();
        }

        public void Rotate() {
            rigidbody.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cachedDirection), rotateSpeed * Time.deltaTime);
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
            if (agent && agentTarget)
            {
                agent.destination = agentTarget.position;
                if (agent.remainingDistance >= reachDistance)
                {
                    agent.Resume();
                }
            }
            else if (rigidbody)
            {
                Vector3 velocity = rigidbody.velocity;
                float ySpeed = velocity.y;
                velocity.y = 0;

                if (velocity.magnitude > speed)
                    velocity = velocity.normalized * speed;

                velocity.y = ySpeed;
                rigidbody.velocity = velocity;
            }
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