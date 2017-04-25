using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;
using Crab.Utils;

public enum ProjectileType {
    Bullet,
    Missile
}

public class Bullet : MonoBehaviour {

    [System.NonSerialized]
    public Entity owner;

    public float initialForce = 300;
    public float lifeTime = 5f;

    protected new Rigidbody rigidbody;
    protected Delay destroyDelay;

	// Use this for initialization
	protected virtual void Start () {
        //Setup references
        rigidbody = GetComponent<Rigidbody>();

        //Setup the bullet for projectile pool usage
        OnStart();
        destroyDelay = new Delay((int)(lifeTime * 1000), true);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (destroyDelay.Over()) {
            //If destroy time has passed, destroy the bullet
            Destroy(gameObject);
        }
	}

    //Reset or secondary start for projectile pool usage
    public virtual void OnStart()
    {
        rigidbody.AddForce(transform.forward * initialForce);
    }
}
