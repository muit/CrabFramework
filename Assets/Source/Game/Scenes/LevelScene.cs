using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Crab;
using Crab.Utils;

public enum LevelFinishReason {
    None,
    TimePassed,
    AllEnemiesKilled,
    PlayerDied
}

public class LevelScene : SceneScript {

    public float roundDuration = 35;
    public float transitionDuration = 2;
    public List<Spawner> spawners;

    [NonSerialized]
    public Delay finishTimer;
    Delay transitionTimer;
    AYSGameInstance GameInstance;

    LevelFinishReason finishReason = LevelFinishReason.None;

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

            Time.timeScale = 0.2f;
            FinishRound(LevelFinishReason.TimePassed);
        }
        if (transitionTimer.Over()) {
            transitionTimer.Reset();
            
            //Finished Successfully
            GameInstance.FinishLevel(finishReason);
            Time.timeScale = 1f;
        }
    }


    public void FinishRound(LevelFinishReason reason) {
        transitionTimer.Start(transitionDuration);
        finishReason = reason;
    }

    public void PlayerDied()
    {
        Debug.Log("Killed");
        FinishRound(LevelFinishReason.PlayerDied);
    }
}
