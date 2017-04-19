using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;
using Crab.Utils;

public class AYSPlayerController : PlayerController {

    public Transform fireLocation;

    public Bullet bulletPrefab;

    public int fireRatePerSecond = 4;
    public int bulletPoolSize = 40;



    BulletPool bullets;

    Delay fireDelay;



    protected override void Awake() {
        base.Awake();
        bullets = new BulletPool(bulletPoolSize);
        fireDelay = new Delay(1000 / fireRatePerSecond);
    }

    protected override void Update()
    {
        base.Update();

        //If we pressed fire and delay is over
        bool fire = Input.GetAxis("Fire1") > 0f;

        if (fire && (fireDelay.Over() || !fireDelay.IsStarted()))
        {
            Transform fireTransform = fireLocation ? fireLocation : transform;

            bullets.Create(bulletPrefab, fireTransform.position, fireTransform.rotation);

            // Restart delay
            fireDelay.Start();
        }
        else if (!fire)
        {
            fireDelay.Reset();
        }
    }
}
