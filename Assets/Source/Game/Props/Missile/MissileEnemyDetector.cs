using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;

public class MissileEnemyDetector : MonoBehaviour {

    public bool scanForTargets = true;
    public bool onlyScanWithoutTarget = true;

    Missile owner;

	// Use this for initialization
	void Start () {
        owner = GetComponentInParent<Missile>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) {
        if (owner && scanForTargets)
        {
            if (!owner.Target || !onlyScanWithoutTarget)
            {
                //Set new target
                owner.Target = col.transform;
            }
        }
    }
}
