using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;
using Crab.Utils;

public class AYSPlayerController : PlayerController {

    [Header("Movement")]
    public LayerMask clickRayCollidesWith;

    [Header("Weapons")]
    public ProjectileType usedProjectile = ProjectileType.Bullet;

    [Header("Bullets")]
    public Transform bulletFireLocation;
    public Bullet bulletPrefab;

    [Header("Missiles")]
    public Transform missileFireLocation;
    public Missile missilePrefab;

    public int fireRatePerSecond = 4;
    public int bulletPoolSize = 40;



    BulletPool bullets;
    BulletPool missiles;

    Delay fireDelay;



    protected override void Awake() {
        base.Awake();

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
        bool leftClick = Input.GetMouseButton(0);

        if (leftClick)
        {
            // Get camera from cache and create a ray from mouse's position
            Ray ray = Cache.Get.camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 300, clickRayCollidesWith))
            {
                //If we hit
                Entity focusedEntity = hit.collider.GetComponent<Entity>();
                if (focusedEntity && focusedEntity.IsEnemyOf(me))
                {
                    Vector3 fireDirection = focusedEntity.transform.position - me.transform.position;
                    HandleFiring(fireDirection, focusedEntity);
                }
                else {
                    //Move to point in the navigation mesh
                    me.Movement.AIMove(hit.point);
                }
            }


        }
    }

    ///////////////////////////////////////////////////////////////////////////
    // FIRING
    private void HandleFiring(Vector3 direction, Entity target = null) {
        //If we pressed fire and delay is over
        if (fireDelay.Over() || !fireDelay.IsStarted())
        {
            if (usedProjectile == ProjectileType.Bullet)
                FireBullet(direction);
            else
                FireMissile(target.transform);

            // Restart delay
            fireDelay.Start();
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
}
