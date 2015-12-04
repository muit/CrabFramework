using UnityEngine;
using Crab.Controllers;
using Crab.Events;

public class SceneScript : MonoBehaviour
{
    // Static singleton property
    public static SceneScript Instance { get; private set; }

    void Awake()
    {
        // Check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    [System.NonSerialized]
    public ESpawn spawn;
    [System.NonSerialized]
    public PlayerController player;
    [System.NonSerialized]
    public CameraMovement camera;

    void Start()
    {
        spawn = FindObjectOfType<ESpawn>();
        BeforeGameStart();

        if (!player)
        {
            player = FindObjectOfType<PlayerController>();
        }

        if (!camera)
        {
            camera = FindObjectOfType<CameraMovement>();
            if (camera) camera.SetTarget(player);
        }

        OnGameStart(player);
    }

    public void LoadScene(int scene)
    {
        Application.LoadLevelAsync(scene);
    }


    //Events
    protected virtual void BeforeGameStart() { }
    protected virtual void OnGameStart(PlayerController player) {}
}