using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab.Utils;

public class Bullet : MonoBehaviour {

    public float initialForce = 300;
    public float lifeTime = 5f;

    new Rigidbody rigidbody;
    Delay destroyDelay;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        Reset();
        destroyDelay = new Delay((int)(lifeTime * 1000), true);
	}
	
	// Update is called once per frame
	void Update () {
        if (destroyDelay.Over()) {
            Destroy(gameObject);
        }
	}

    public void Reset()
    {
        rigidbody.AddForce(transform.forward * initialForce);
    }
}
