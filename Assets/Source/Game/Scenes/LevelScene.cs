using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Crab;
using Crab.Utils;

public enum LevelFinishReason {
    TimePassed,
    AllEnemiesKilled,
    PlayerDied
}

public class LevelScene : SceneScript {

    public float roundDuration;
    public List<Spawner> spawners;

    Delay finishTimer;
    AYSGameInstance GameInstance;

    protected override void OnGameStart(PlayerController Player)
    {
        GameInstance = FindObjectOfType<AYSGameInstance>();

        finishTimer = new Delay((int)(roundDuration*1000), true);

        foreach (Spawner spawner in spawners) {
            spawner.Activate();
        }
    }

    protected override void Update() {
        if (finishTimer.Over()) {
            if (Cache.Get.player.Me.IsAlive()) {
                //Finish Successfully
                //Call GameInstance
            }
        }
    }
}
