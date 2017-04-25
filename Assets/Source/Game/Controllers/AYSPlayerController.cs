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
        ///////////////////////////////////////////////////////////////////////
        // MOVING
        bool leftClick = Input.GetMouseButtonDown(0);
        if (leftClick)
        {
            // Get camera from cache and create a ray from mouse's position
            Ray ray = Cache.Get.camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 300, clickRayCollidesWith))
            {
                Entity focusedEntity = hit.collider.GetComponent<Entity>();
                Debug.Log(hit.transform.name);
                Debug.DrawLine(ray.origin, hit.point, Color.blue, 2);
                if (focusedEntity && focusedEntity.IsEnemyOf(me))
                {
                    HandleFiring(focusedEntity.transform.position - me.transform.position);
                }
                else {
                    me.Movement.AIMove(hit.point);
                }
            }


        }
    }

    ///////////////////////////////////////////////////////////////////////////
    // FIRING
    private void HandleFiring(Vector3 direction) {
        //If we pressed fire and delay is over
        if (fireDelay.Over() || !fireDelay.IsStarted())
        {
            if (usedProjectile == ProjectileType.Bullet)
                FireBullet(direction);
            else
                FireMissile(null);

            // Restart delay
            fireDelay.Start();
        }
    }
    
    public void FireBullet(Vector3 direction)
    {
        //Use our own transform if theres no fire location
        Transform fireTransform = bulletFireLocation != null ? bulletFireLocation : transform;

        //Fire bullet
        Quaternion directionRot =Quaternion.FromToRotation(me.transform.forward, direction);
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
