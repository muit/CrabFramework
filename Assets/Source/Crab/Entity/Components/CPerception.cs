using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Crab.Debug;

namespace Crab.Entities
{
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(SphereCollider))]
    [DisallowMultipleComponent]
    public class CPerception : MonoBehaviour
    {
        private Entity me;

        public float viewDistance = 5.0f;
        public float lostDistance = 7.0f;
        public float fieldOfViewAngle = 110f;           // Number of degrees, centred on forward, for the enemy see.

        [Header("Events")]
        //public UnityEvent perceptionUpdate;
        public PerceptionDetectedEvent onEntityDetected;
        public PerceptionDetectedEvent onEntityLost;

        new private SphereCollider collider;  // Reference to the sphere collider trigger component.

        private HashSet<Entity> nearTargets = new HashSet<Entity>();
        private HashSet<Entity> visibleTargets = new HashSet<Entity>();
        private Dictionary<Entity, LastSight> lastSights = new Dictionary<Entity, LastSight>();

        private DetectionCone detectionCone;


        void Awake()
        {
            me = GetComponent<Entity>();

            // Setting up the references.
            collider = GetComponent<SphereCollider>();

            if (detectionCone == null) detectionCone = new DetectionCone();
        }

        private float lastDistance = 0;

        void Update()
        {
            if (viewDistance != lastDistance)
            {
                lastDistance = viewDistance;

                collider.isTrigger = true;
                collider.radius = viewDistance / transform.localScale.y;
            }


            List<Entity> lostEntities = new List<Entity>();

            foreach (Entity visibleEntity in visibleTargets)
            {
                if (visibleEntity)
                {
                    float sqrDistance = Vector3.SqrMagnitude(transform.position - visibleEntity.transform.position);
                    if (sqrDistance > lostDistance * lostDistance)
                    {
                        lostEntities.Add(visibleEntity);
                    }
                }
            }

            foreach (Entity lostEntity in lostEntities) {
                //lost Target
                if (visibleTargets.Remove(lostEntity))
                {
                    if (me.IsAI())
                    {
                        me.ai.StopCombatWith(lostEntity);
                    }
                    //Error: Failed to convert parameters
                    //onEntityDetected.Invoke(target);
                }
            }
        }


        void OnTriggerStay(Collider other)
        {
            Entity target = other.GetComponent<Entity>();
            if (!target || target == me)
                return;

            if (!nearTargets.Contains(target))
            {
                nearTargets.Add(target);
            }

            // Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            // If the angle between forward and where the player is, is less than half the angle of view...
            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                // ... and if a raycast towards the player hits something...
                if (Physics.Raycast(transform.position, direction.normalized, out hit, collider.radius))
                {
                    if (hit.collider.GetComponentInParent<Entity>() == target)
                    {
                        if (visibleTargets.Add(target))
                        {
                            if (me.IsAI())
                            {
                                me.ai.StartCombatWith(target);
                            }
                            //Error: Failed to convert parameters
                            //onEntityDetected.Invoke(target);
                        }
                    }
                }
            }
        }


        void OnTriggerExit(Collider other)
        {
            Entity target = other.GetComponent<Entity>();
            if (!target)
                return;

            nearTargets.Remove(target);
        }

        //Debug
        void OnDrawGizmos()
        {
            Color gizmosColor = Gizmos.color;
            if (detectionCone != null)
            {
                Gizmos.color = new Color(0, 0.8f, 0, 0.6f);
                detectionCone.Draw(transform.position, transform.rotation.eulerAngles.y, fieldOfViewAngle, viewDistance);
            }

            Gizmos.color = Color.red;
            foreach (Entity entity in visibleTargets)
            {
                Gizmos.DrawLine(transform.position, entity.transform.position);
            }

            UnityEditor.Handles.color = new Color(0, 1f, 0, 0.75f);
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector2.up, viewDistance);
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector2.up, lostDistance);

            Gizmos.color = gizmosColor;
        }
    }

    struct LastSight
    {
        Vector3 position;
        Quaternion rotation;
    }

    [System.Serializable]
    public class PerceptionDetectedEvent : UnityEvent<Entity> { }
}