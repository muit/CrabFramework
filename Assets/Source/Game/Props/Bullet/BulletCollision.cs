using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger || col.transform.parent == this)
            return;

        Bullet parent = GetComponentInParent<Bullet>();


        if (parent) {
            parent.OnHit(col);
        }
    }
}
