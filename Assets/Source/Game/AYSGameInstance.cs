using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;

public class AYSGameInstance : GameInstance {

    public int maximumLevels = 4;
    public string levelScene = "Game";
    public string bossScene  = "Game";


    public int currentLevel = 0;
    bool bossPhase = false;


    public void ResetRun() {
        currentLevel = 0;
        bossPhase = false;

        LoadScene(levelScene);
    }

    public void FinishLevel(LevelFinishReason reason) {
        if (reason == LevelFinishReason.TimePassed) {
            if (IsInBossLevel()) {
                Debug.LogWarning("Tried to finish a round from a boss level.");
                return;
            }

            ++currentLevel;
            
            if (currentLevel < maximumLevels) {
                // go next level
                LoadScene(levelScene);
            }
            else {
                //go Boss phase
                bossPhase = true;
                LoadScene(bossScene);
            }

            return;
        }

        //Failed
        if (reason == LevelFinishReason.PlayerDied)
        {
            ResetRun();
        }
    }


    public int Level {
        get { return currentLevel; }
    }

    public bool IsInBossLevel() {
        return bossPhase;
    }
}
