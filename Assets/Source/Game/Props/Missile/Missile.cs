using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;

public class Missile : Bullet {
    protected Transform target;
    
    public Transform Target
    {
        set {
            if (value)
            {
                Entity newTarget = value.GetComponent<Entity>();
                //Check factions
                if (newTarget && (!owner || newTarget.IsEnemyOf(owner)))
                    target = value;
            }
        }
        get {
            return target;
        }
    }

    public float initialSpeed = 1;
    public float forwardForce = 100;
    public float directionForce = 0.1f;
    public AnimationCurve directionCofByLife;

    new ConstantForce constantForce;

    float startTime;

    protected override void Start()
    {
        constantForce = GetComponent<ConstantForce>();
        base.Start();
    }

    public override void OnStart()
    {
        base.OnStart();

        //Reset target
        target = null;

        rigidbody.velocity = transform.forward * initialSpeed;
        startTime = Time.time;
        constantForce.relativeForce = new Vector3(0, 0, forwardForce);
    }

    protected override void Update()
    {
        base.Update();

        if (!target)
            return;
        
        Vector3 BA = target.position - transform.position;
        //Vector3 perfectTorque = BA.normalized - transform.forward;

        //Get multiplier by lifetime
        float Cof = directionCofByLife.Evaluate(Time.time - startTime);
        Quaternion LookRotation = Quaternion.LookRotation(BA, Vector3.up);
        
        rigidbody.rotation = Quaternion.LerpUnclamped(transform.rotation, LookRotation, Cof * directionForce);
    }
}
