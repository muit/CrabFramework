using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    string gameScenePath = "Scenes/Game";

    public void Play() {
        SceneManager.LoadSceneAsync(gameScenePath);
    }

    public void Exit() {
        Application.Quit();
    }
}
