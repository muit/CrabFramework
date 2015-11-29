using System;
using UnityEngine;
using System.Collections;
using PicaVoxel;
using UnityEngine.UI;
using Random = System.Random;

public class DerbyCar : MonoBehaviour
{
    private Exploder exploder;

    public Color Color;

    public bool IsPlayerControlled;

    private float steerAmount;

    private float resetTime = 0f;
    private bool resetting = false;
    private Vector3 targetResetPosition;
    private Quaternion targetResetRotation;

    private Camera mainCamera;
    private Vector3 cameraStartPosition;

    private Rigidbody rigidBody;

    // AI vars
    private DerbyCar targetCar;
    private float stuckTime = 0f;
    private float reverseTime = 0f;

 
	void Start ()
	{
        // Setting the gravity manually because the demo games are shared in one project
        Physics.gravity = new Vector3(0f,-50f,0f);

        // Find this car's Exploder so we can destroy some voxels when it hits something
	    exploder = transform.FindChild("Exploder").GetComponent<Exploder>();

	    rigidBody = GetComponent<Rigidbody>();

        // Set the car's tint color!
        Material m = new Material(transform.FindChild("Body").GetComponent<Volume>().Material);
        m.SetColor("_Tint", Color);
	    transform.FindChild("Body").GetComponent<Volume>().Material = m;
        transform.FindChild("Body").GetComponent<Volume>().UpdateAllChunks();

        // If this car is player controllerd, it should have a camera attached
	    if (IsPlayerControlled)
	    {
	        mainCamera = transform.FindChild("Main Camera").GetComponent<Camera>();
	        cameraStartPosition = mainCamera.transform.localPosition;
	    }
	}
	
	void Update () {
	    if (!resetting)
	    {
	        if (IsPlayerControlled)
	        {
                // Turn the wheels and add acceleration/braking as per player input
	            if (Input.GetAxis("Horizontal")<0f)
	                steerAmount -= 0.1f;
	            else if (Input.GetAxis("Horizontal")>0f)
	                steerAmount += 0.1f;
	            else if (steerAmount < 0f) steerAmount += 0.05f;
	            else if (steerAmount > 0f) steerAmount -= 0.05f;

	            if (Input.GetAxis("Vertical")>0f)
	                rigidBody.AddForce(transform.forward*25f, ForceMode.Acceleration);
	            if (Input.GetAxis("Vertical")<0f)
	                rigidBody.AddForce(-transform.forward*15f, ForceMode.Acceleration);

                // Do something with the camera
	            mainCamera.transform.localPosition = Vector3.Slerp(mainCamera.transform.localPosition, cameraStartPosition + new Vector3(steerAmount*(rigidBody.velocity.magnitude*0.1f), 0f, 0f), Time.deltaTime * 10f);
                mainCamera.transform.localRotation = Quaternion.RotateTowards(mainCamera.transform.localRotation, Quaternion.Euler(20, 0, steerAmount*5f), Time.deltaTime * 10f);
	        }
	        else
	        {
                // Do some AI!

                // Find a car to target (but not myself!)
	            if (targetCar == null)
	            {
	                GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
	                int newTarget = UnityEngine.Random.Range(0, cars.Length);
	                if (cars[newTarget] != gameObject)
	                {
	                    targetCar = cars[newTarget].GetComponent<DerbyCar>();
	                }
	            }
	            else
	            {
                    // Attempt to steer toward target car
	                Vector3 v3angle = targetCar.transform.position - transform.position;
	                float angle = Vector3.Cross(transform.forward, v3angle).y;
	             
	                if (angle > 2f)
	                {
	                    steerAmount += 0.1f;
	                }
                    else if (angle < 2f)
                    {
                        steerAmount -= 0.1f;
                    }
                    else if (steerAmount < 0f) steerAmount += 0.05f;
                    else if (steerAmount > 0f) steerAmount -= 0.05f;

                    if(reverseTime<=0f)
                        rigidBody.AddForce(transform.forward * 25f, ForceMode.Acceleration);
                    else
                        rigidBody.AddForce(-transform.forward * 15f, ForceMode.Acceleration);

	            }

                // If we're not moving, add to the stuck timer
	            if (rigidBody.velocity.magnitude > 1f)
	                stuckTime = 0f;
	            else
	                stuckTime += Time.deltaTime;

                // If we've been stuck for 3 seconds, allow us to reverse for 5 seconds
	            if (stuckTime > 3f)
	            {
	                stuckTime = 0f;
	                reverseTime = 5f;
	            }

                // If we've been stuck for 8 seconds, change target
	            if (stuckTime > 8f)
	            {
	                targetCar = null;
	            }

	            if (reverseTime > 0f)
	                reverseTime -= Time.deltaTime;

                // Randomly change target now and then
	            if (UnityEngine.Random.Range(0, 500) == 0)
	                targetCar = null;
	        }

	    }

        // Steering
        steerAmount = Mathf.Clamp(steerAmount, -1f, 1f);
	    transform.FindChild("WheelFL").localRotation = Quaternion.Euler(0, 0f + (steerAmount*10f), 0f);
	    transform.FindChild("WheelFR").localRotation = Quaternion.Euler(0, 0f + (steerAmount*10f), 0f);

        // Check to see if the car has fallen over (greater than 80 degrees of tilt around Z)
        // If it has, wait 3 seconds before resetting the position
	    if (Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.z, 0f))>80f && resetTime <= 0f && !resetting)
	        resetTime = 3f;

        // We've been stuck for long enough!
	    if (resetTime > 0f)
	    {
	        resetTime -= Time.deltaTime;
	        if (resetTime <= 0f)
	        {
	            resetting = true;

                // Set position and rotation targets to lerp towards
	            targetResetPosition = transform.position + new Vector3(0f, 2f, 0f);
	            targetResetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,0f);
	        }
	    }

        // Reset the car's position
	    if (resetting)
	    {
            // Turn off gravity while we're being lifted up
	        GetComponent<Rigidbody>().useGravity = false;

            // Lerp towards the correct rotation and a raised position
	        transform.position = Vector3.Slerp(transform.position, targetResetPosition, Time.deltaTime * 5f);
	        transform.rotation = Quaternion.Lerp(transform.rotation,targetResetRotation, Time.deltaTime * 5f);

            // Once the rotation is upright again, drop the car
            if (Vector3.Distance(transform.position, targetResetPosition) < 0.2f && Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.z, 0f)) < 5f)
	        {
	            resetting = false;
	            GetComponent<Rigidbody>().useGravity = true;
	        }
	    }
	}

    void OnCollisionEnter(Collision col)
    {
        // Something has collided with the car, so we'll find out where in the scene the collision occured
        // We'll average out the contact points to give a median position
        Vector3 avg = Vector3.zero;
        foreach (ContactPoint cp in col.contacts) avg += cp.point;
        if(col.contacts.Length>1)
            avg /= (float)col.contacts.Length;
            
        // Set the Exploder's position to the average collision position
        exploder.transform.position = avg;

        // Just for effect, we're going to move the collision point up a couple of voxels:
        exploder.transform.position += new Vector3(0f,0.25f,0f);

        // We'll give our explosion particles some upward velocity - also for effect
        exploder.Explode(new Vector3(0f,7f,0f));
    }
}
