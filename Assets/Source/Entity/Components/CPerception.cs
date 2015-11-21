using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(SphereCollider))]
[DisallowMultipleComponent]
public class CPerception : MonoBehaviour {
    private Entity me;

	public float viewDistance = 5.0f;
	public float fieldOfViewAngle = 110f;           // Number of degrees, centred on forward, for the enemy see.

    [Header("Events")]
    //public UnityEvent perceptionUpdate;
    public PerceptionDetectedEvent onEntityDetected;
    public PerceptionDetectedEvent onEntityLost;

    private SphereCollider collider;                     // Reference to the sphere collider trigger component.

	private HashSet<Entity> nearTargets = new HashSet<Entity>();
	private HashSet<Entity> visibleTargets = new HashSet<Entity>();
	private Dictionary<Entity, LastSight> lastSights = new Dictionary<Entity, LastSight>();
	
	void Awake ()
	{
		me = GetComponent<Entity>();

		// Setting up the references.
		collider = GetComponent<SphereCollider>();
		collider.isTrigger = true;
		collider.radius = viewDistance / transform.localScale.y;
	}
	
	void OnTriggerStay (Collider other)
	{
		Entity target = other.GetComponent<Entity> ();
		if (!target || target == me)
			return;

        if (!nearTargets.Contains(target)) {
            nearTargets.Add(target);
        }

		// Create a vector from the enemy to the player and store the angle between it and forward.
		Vector3 direction = other.transform.position - transform.position;
		float angle = Vector3.Angle(direction, transform.forward);
		
		// If the angle between forward and where the player is, is less than half the angle of view...
		if(angle < fieldOfViewAngle * 0.5f)
		{
			RaycastHit hit;
			
			// ... and if a raycast towards the player hits something...
			if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, collider.radius))
			{
				if(hit.collider.GetComponent<Entity>() == target) {
                    if (visibleTargets.Add(target)) {
                        onEntityDetected.Invoke(target);
                        Debug.Log("Found you!");
                    }
                }
			}
		}
	}
	
	
	void OnTriggerExit (Collider other)
	{
		Entity target = other.GetComponent<Entity> ();
		if (!target)
			return;

        nearTargets.Remove (target);

        if (visibleTargets.Remove(target)) {
            onEntityDetected.Invoke(target);
            Debug.Log("Lost ;(");
        }
    }
}

struct LastSight {
	Vector3 position;
	Quaternion rotation;
}

[System.Serializable]
public class PerceptionDetectedEvent : UnityEvent<Entity> { }