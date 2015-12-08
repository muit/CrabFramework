using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public EntityController motion;

	public Vector3 offset;
	[Header("Position")]
	public float distance = 4;
	public float inclination = 45;
	[Header("Rotation")]
	public bool followsMotionRotation = false;
	public float yRotation = 180;
	private float yRadRotation = 0;
	[Header("Movement")]
	public float moveDamping = 2;
	public float lookDamping = 2;
	public float teleportAtDistance = 100;

	void LateUpdate () {
		if (motion != null) {
			UpdatePosition(motion.transform);
		}
		else if(SceneScript.Instance.spawn){
			UpdatePosition(SceneScript.Instance.spawn.transform);
		}
	}

	public void SetTarget(EntityController _motion)
	{
		motion = _motion;
	}

	private void UpdatePosition(Transform trans)
	{
		if (!trans)
		{
			Debug.LogWarning("Camera target transform is null.");
			return;
		}

		if (followsMotionRotation) {
			yRotation = Mathf.Rad2Deg * trans.rotation.y + 180;
			Debug.Log(trans.rotation);
		}

		Vector3 position = trans.position + offset;
		Quaternion targetRotation = Quaternion.LookRotation(position - transform.position);

		position.y += distance * Mathf.Sin((90 - inclination) * 2 * Mathf.PI / 360);
		float catet = distance * Mathf.Cos((90 - inclination) * 2 * Mathf.PI / 360);

		yRadRotation = Mathf.Deg2Rad * yRotation;
		position.x += catet * Mathf.Cos(yRadRotation);
		position.z += catet * Mathf.Sin(yRadRotation);

		//Smooth LookAt
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookDamping * Time.deltaTime);

		//Temporal fix camera stutting
		transform.position = position;

		/*
		//Teleport camera if is too far
		if (Vector3.Distance(trans.position, transform.position) > teleportAtDistance)
			transform.position = position;
		else
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * moveDamping);
		*/
	}

	[ContextMenu("Do Something")]
	void DoSomething()
	{
		Debug.Log("Perform operation");
	}
}
