using UnityEngine;
using Crab;

public class Cache : MonoBehaviour {
    [Header("Character Prefabs")]
    public PlayerController player;

    //[Header("Object Prefabs")]
    
    [Header("References")]
    public new Camera camera;

    AYSGameInstance _gameInstance;

    public AYSGameInstance gameInstance {
        get {
            return _gameInstance ? _gameInstance : _gameInstance = FindObjectOfType<AYSGameInstance>();
        }
    }

    LevelScene _levelScene;

    public LevelScene levelScene
    {
        get
        {
            return _levelScene ? _levelScene : _levelScene = GetComponent<LevelScene>();
        }
    }


    //Singletone
    private static Cache instance;
    public static Cache Get
    {
        get {
            return instance? instance : instance = FindObjectOfType<Cache>();
        }
        private set { instance = value; }
    }
}
