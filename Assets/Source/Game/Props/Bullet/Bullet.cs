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


    public ParticleSystem destroyParticlePrefab;
    public float particleRate = 1.0f;

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

    public void OnHit(Collider col) {
        if (col.isTrigger || col.transform.parent == this)
            return;

        Entity entity = col.GetComponent<Entity>();
        //Is entity and is enemy of our owner?
        if (entity && entity != owner && entity.IsEnemyOf(owner))
        {
            //Apply damage and call events
            entity.Damage(damage, owner);

            if (destroyParticlePrefab && Random.Range(0.0f, 1.0f) < particleRate) {
                ParticleSystem.Instantiate(destroyParticlePrefab, transform.position, transform.rotation);
            }
            onHit.Invoke();
            Destroy(gameObject);
        }
    }
}
