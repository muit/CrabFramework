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

    public float roundDuration = 25;
    public float transitionDuration = 2;
    public List<Spawner> spawners;

    Delay finishTimer;
    Delay transitionTimer;
    AYSGameInstance GameInstance;

    protected override void OnGameStart(PlayerController Player)
    {
        GameInstance = FindObjectOfType<AYSGameInstance>();

        finishTimer = new Delay(roundDuration);
        transitionTimer = new Delay(transitionDuration, false);

        foreach (Spawner spawner in spawners) {
            spawner.Activate();
        }
    }

    protected override void Update()
    {
        if (finishTimer.Over())
        {
            finishTimer.Reset();
            transitionTimer.Start(transitionDuration);
            Time.timeScale = 0.2f;
        }
        if (transitionTimer.Over()) {
            transitionTimer.Reset();

            if (Cache.Get.player.Me.IsAlive())
            {
                //Finished Successfully
                GameInstance.FinishLevel(true, LevelFinishReason.TimePassed);
                Time.timeScale = 1f;
            }
        }
    }
}
