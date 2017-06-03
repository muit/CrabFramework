using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;
using Crab.Utils;

public class AYSPlayerController : PlayerController {

    [Header("Movement")]
    public LayerMask clickRayCollidesWith;

    [Header("Bullets")]
    public Transform bulletFireLocation;
    public Bullet bulletPrefab;

    [Header("Missiles")]
    public Transform missileFireLocation;
    public Missile missilePrefab;

    public int fireRatePerSecond = 4;
    public int fireMissileLivePct = 25;
    public int bulletPoolSize = 40;

    [Header("Dodge")]
    public float dodgeForward = 2;
    public float dodgeSide = 2;
    public float dodgeForwardMax = 1.5f;
    public float dodgeSideMax = 1.5f;
    public float dodgeDuration = 1f;
    public float dodgeFidelity = 1f;


    BulletPool bullets;
    BulletPool missiles;

    Delay fireDelay;

    bool dodging = false;
    float dodgeCurrentDuration = 0;
    float dodgeSpeed= 0;
    float sideCofBeforeDodge = 0.0f;
    float forwardCofBeforeDodge = 0.0f;


    protected override void Awake() {
        base.Awake();

        Cache.Get.player = this;

        //Create pools
        bullets = new BulletPool(bulletPoolSize);
        missiles = new BulletPool(bulletPoolSize);

        fireDelay = new Delay(1000 / fireRatePerSecond);
    }

    protected override void Update()
    {
        if (!me.IsAlive())
            return;

        ///////////////////////////////////////////////////////////////////////
        // MOVING

        float sideCof = Input.GetAxis("Horizontal");
        float forwardCof = Input.GetAxis("Vertical");

        if (dodging)
        {
            //Clean this mess!

            dodgeCurrentDuration += Time.deltaTime;

            float dodgeDelta = dodgeCurrentDuration / dodgeDuration;
            bool dodgeIsStarting = dodgeDelta < 0.5f;


            float dodgeSideCof    = Mathf.Clamp(Mathf.Lerp(0, dodgeSide,    1-Mathf.Abs(dodgeDelta * 2 - 1)), 0, dodgeSideMax);
            float dodgeForwardCof = Mathf.Clamp(Mathf.Lerp(0, dodgeForward, 1-Mathf.Abs(dodgeDelta * 2 - 1)), 0, dodgeForwardMax);

            me.Movement.Move(
                sideCofBeforeDodge > 0 ?    Mathf.Max(dodgeSideCof, sideCofBeforeDodge * dodgeFidelity)       : Mathf.Min(-dodgeSideCof, sideCofBeforeDodge * dodgeFidelity),
                forwardCofBeforeDodge > 0 ? Mathf.Max(dodgeForwardCof, forwardCofBeforeDodge * dodgeFidelity) : Mathf.Min(-dodgeForwardCof, forwardCofBeforeDodge * dodgeFidelity)
            );


            if (dodgeDelta >= 1)
            {
                dodging = false;
            }
        }
        else
        {
            if ((forwardCof != 0 || sideCof != 0) && Input.GetKeyDown(KeyCode.Space))
            {
                dodging = true;
                dodgeSpeed = 0f;
                dodgeCurrentDuration = 0f;

                sideCofBeforeDodge    = sideCof;
                forwardCofBeforeDodge = forwardCof;
            }

            me.Movement.Move(sideCof, forwardCof);
        }


        Vector3 fireDirection = me.transform.forward;

        bool leftClick =  Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButton(1);

        if (leftClick)  HandleFiring(fireDirection);
        if (rightClick) HandleFiring(fireDirection, ProjectileType.Missile);
    }

    ///////////////////////////////////////////////////////////////////////////
    // FIRING
    private void HandleFiring(Vector3 direction, ProjectileType projectile = ProjectileType.Bullet, Entity target = null) {
        //If we pressed fire and delay is over
        if (fireDelay.Over() || !fireDelay.IsStarted())
        {
            // Restart delay
            fireDelay.Start();
            
            if (target && target.Attributes.LivePercentage <= fireMissileLivePct)
            {
                FireMissile(target.transform);
                return;
            }

            if (projectile == ProjectileType.Bullet)
                FireBullet(direction);
            else
                FireMissile(direction);
        }
    }
    
    public void FireBullet(Vector3 direction)
    {
        //Use our own transform if theres no fire location
        Transform fireTransform = bulletFireLocation != null ? bulletFireLocation : transform;

        //Fire bullet
        Quaternion directionRot =Quaternion.LookRotation(direction);
        bullets.Create(bulletPrefab, fireTransform.position, directionRot, me);
    }

    public void FireMissile(Transform target)
    {
        //Use our own transform if theres no fire location
        Transform fireTransform = missileFireLocation != null ? missileFireLocation : transform;

        //Fire missile
        Missile missile = (Missile)missiles.Create(missilePrefab, fireTransform.position, fireTransform.rotation, me);
        //Set target to follow
        missile.Target = target;
    }

    public void FireMissile(Vector3 direction)
    {
        //Use our own transform if theres no fire location
        Transform fireTransform = missileFireLocation != null ? missileFireLocation : transform;

        //Fire missile
        Quaternion directionRot = Quaternion.LookRotation(direction);
        Missile missile = (Missile)missiles.Create(missilePrefab, fireTransform.position, directionRot, me);
    }
}
