using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;

public class AYSGameInstance : GameInstance {

    public int maximumLevels = 4;
    public string levelScene = "Game";
    public string bossScene  = "Game";


    int currentLevel = 0;
    bool bossPhase = false;


    public void ResetRun() {
        currentLevel = 0;
        bossPhase = false;

        LoadScene(levelScene);
    }

    public void FinishLevel(bool success, LevelFinishReason reason) {
        if (success) {
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
        }

        LoadScene(levelScene);
    }


    public int Level {
        get { return currentLevel; }
    }

    public bool IsInBossLevel() {
        return bossPhase;
    }
}
