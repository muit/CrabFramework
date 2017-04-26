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
    public float angularAmount = 50f;
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

        rigidbody.velocity = transform.forward * initialSpeed;
        startTime = Time.time;
        constantForce.relativeForce = new Vector3(0, 0, forwardForce);
    }

    protected override void Update()
    {
        base.Update();

        Debug.Log(target);
        if (!target)
            return;
        
        Vector3 BA = target.position - transform.position;

        Quaternion LookRotation = Quaternion.LookRotation(BA);
        
        //Get multiplier by lifetime
        float Cof = directionCofByLife.Evaluate(Time.time - startTime);
        Debug.DrawLine(transform.position, transform.position + LookRotation * Vector3.forward, Color.blue);

        rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, LookRotation, Time.deltaTime * angularAmount * Cof);
    }
}
