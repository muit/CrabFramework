using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Crab;
using Crab.Utils;


[Serializable]
public class ProjectilePreset<T>
    where T : Bullet
{
    public Transform fireLocation;
    public T prefab;
    public int fireRate = 4;
}

[Serializable]
public class BulletPreset : ProjectilePreset<Bullet> { }

[Serializable]
public class MissilePreset : ProjectilePreset<Bullet> { }


public class AYSPlayerController : PlayerController {

    [Header("Movement")]
    public LayerMask clickRayCollidesWith;

    [Header("Projectiles")]
    public BulletPreset  bulletPreset;
    public MissilePreset missilePreset;

    //public int fireMissileLivePct = 25;
    public int bulletPoolSize = 40;

    [Header("Dodge")]
    public float dodgeForward = 2;
    public float dodgeSide = 2;
    public float dodgeForwardMax = 1.5f;
    public float dodgeSideMax = 1.5f;
    public float dodgeDuration = 1f;
    public float dodgeFidelity = 1f;
    public float dodgeCooldown = 1f;


    BulletPool bullets;
    BulletPool missiles;

    Delay fireDelay;
    Delay dodgeCooldownDelay;

    bool dodging = false;
    float dodgeCurrentDuration = 0;
    float dodgeSpeed= 0;
    Vector2 beforeDodgeDir;


    protected override void Awake() {
        base.Awake();

        Cache.Get.player = this;

        //Create pools
        bullets = new BulletPool(bulletPoolSize);
        missiles = new BulletPool(bulletPoolSize);

        fireDelay = new Delay(500);
        dodgeCooldownDelay = new Delay(0, false);
    }

    protected override void Update()
    {
        if (!me.IsAlive())
        {
            me.Movement.Move(0,0);
            return;
        }

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

            Vector2 beforeDodgeDirNm = beforeDodgeDir.normalized;

            float dodgeSideCof    = CalculateDodgeSpeed(dodgeSide,    dodgeSideMax,    dodgeDelta, beforeDodgeDir.x, beforeDodgeDirNm.x);
            float dodgeForwardCof = CalculateDodgeSpeed(dodgeForward, dodgeForwardMax, dodgeDelta, beforeDodgeDir.y, beforeDodgeDirNm.y);

            me.Movement.Move(
                dodgeSideCof,
                dodgeForwardCof
            );


            if (dodgeDelta >= 1)
            {
                dodging = false;
                dodgeCooldownDelay.Start(dodgeCooldown);
            }
        }
        else
        {
            if (dodgeCooldownDelay.Over() || !dodgeCooldownDelay.IsStarted())
            {
                if ((forwardCof != 0 || sideCof != 0)
                  && Input.GetKey(KeyCode.Space))
                {
                    dodging = true;
                    dodgeSpeed = 0f;
                    dodgeCurrentDuration = 0f;

                    beforeDodgeDir = new Vector2(sideCof, forwardCof);
                }
            }

            me.Movement.Move(sideCof, forwardCof);
        }


        Vector3 fireDirection = me.transform.forward;

        bool leftClick =  Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButton(1);

        if (leftClick)  HandleFiring(fireDirection);
        if (rightClick) HandleFiring(fireDirection, ProjectileType.Missile);
    }


    private float CalculateDodgeSpeed(float dodgeSpeed, float dodgeSpeedMax, float dodgeDelta, float lastInput, float lastInputNormalized)
    {
        if (!dodging)
            return 0.0f;

        if (lastInput == 0)
            return 0.0f;

        float direction = lastInput > 0 ? 1 : -1; 

        //Calculate pure dodge speed
        float dodgeSpeedCof = Mathf.Clamp(Mathf.Lerp(0, dodgeSpeed, 1 - Mathf.Abs(dodgeDelta * 2 - 1)), 0, dodgeSpeedMax);

        //Smooth transition into dodge
        return Mathf.Max(dodgeSpeedCof, Mathf.Abs(lastInput) * dodgeFidelity) * lastInputNormalized;
    }

    ///////////////////////////////////////////////////////////////////////////
    // FIRING
    private void HandleFiring(Vector3 direction, ProjectileType projectile = ProjectileType.Bullet, Entity target = null) {
        //If we pressed fire and delay is over
        if (fireDelay.Over() || !fireDelay.IsStarted())
        {
            if (projectile == ProjectileType.Bullet)
                FireBullet(direction);
            else
                FireMissile(direction);
        }
    }
    
    public void FireBullet(Vector3 direction)
    {
        //Use our own transform if theres no fire location
        Transform fireTransform = bulletPreset.fireLocation != null ? bulletPreset.fireLocation : transform;

        //Fire bullet
        Quaternion directionRot =Quaternion.LookRotation(direction);
        bullets.Create(bulletPreset.prefab, fireTransform.position, directionRot, me);

        // Restart delay
        fireDelay.Start(1000 / bulletPreset.fireRate);
    }

    public void FireMissile(Transform target)
    {
        //Use our own transform if theres no fire location
        Transform fireTransform = missilePreset.fireLocation != null ? missilePreset.fireLocation : transform;

        //Fire missile
        Missile missile = (Missile)missiles.Create(missilePreset.prefab, fireTransform.position, fireTransform.rotation, me);
        //Set target to follow
        missile.Target = target;

        // Restart delay
        fireDelay.Start(1000 / missilePreset.fireRate);
    }

    public void FireMissile(Vector3 direction)
    {
        //Use our own transform if theres no fire location
        Transform fireTransform = missilePreset.fireLocation != null ? missilePreset.fireLocation : transform;

        //Fire missile
        Quaternion directionRot = Quaternion.LookRotation(direction);
        Missile missile = (Missile)missiles.Create(missilePreset.prefab, fireTransform.position, directionRot, me);

        // Restart delay
        fireDelay.Start(1000 / missilePreset.fireRate);
    }
}
