using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Crab;

public class InGameUI : MonoBehaviour {

    public Text health;
    public Text level;
    public Text timeRemaining;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Entity player = Cache.Get.player.Me;

        if (player) {
            health.text = ""+player.Attributes.Live;
        }

        level.text = "Level " + (Cache.Get.gameInstance.Level+1);
        timeRemaining.text = Cache.Get.levelScene.finishTimer.TimeRemaining().ToString("F1");
    }
}
