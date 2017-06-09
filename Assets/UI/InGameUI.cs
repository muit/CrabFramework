using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Crab;

public class InGameUI : MonoBehaviour {

    public Text health;
    public Text level;
    public Text timeRemaining;
    AYSGameInstance GI;
    LevelScene LE;

    // Use this for initialization
    void Start () {
        GI = Cache.Get.gameInstance;
        LE = Cache.Get.levelScene;
    }
	
	// Update is called once per frame
	void Update () {
        Entity player = Cache.Get.player.Me;

        if (player) {
            health.text = ""+player.Attributes.Live;
        }
        
        if(GI.IsInBossLevel())
            level.text = "Boss!!";
        else
            level.text = "Level " + (GI.Level+1);

        timeRemaining.text = LE.finishTimer.TimeRemaining().ToString("F1");
    }
}
