using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab.Utils;

public class DestroyInXSeconds : MonoBehaviour {

    public float seconds = 3.0f;

    private Delay delay;

	// Use this for initialization
	void Start () {
        delay = new Delay((int)(seconds*1000), true);
	}
	
	// Update is called once per frame
	void Update () {
        if (delay.Over()) {
            Destroy(gameObject);
        }
	}
}
