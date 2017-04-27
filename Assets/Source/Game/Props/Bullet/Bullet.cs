using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Crab;
using Crab.Utils;

public enum ProjectileType {
    Bullet,
    Missile
}

public class Bullet : MonoBehaviour {

    [System.NonSerialized]
    public Entity owner;

    public int damage = 5;
    public float initialForce = 300;
    public float lifeTime = 5f;

    public UnityEvent onHit;

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

    void OnTriggerEnter(Collider col) {
        if (col.isTrigger)
            return;

        Entity entity = col.GetComponent<Entity>();
        //Is entity and is enemy of our owner?
        if (entity && entity != owner && entity.IsEnemyOf(owner))
        {
            //Debug.Log(entity);
            //Apply damage and call events
            entity.Damage(damage, owner);
            onHit.Invoke();
            Destroy(gameObject);
        }
    }
}
