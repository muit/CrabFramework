using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Crab.Controllers;


public class Main : SceneScript {

    protected override void BeforeGameStart()
    {
        player = spawn.Spawn();
    }

    protected override void OnGameStart(PlayerController player) {}

    //GameStats Handling
    //
}
