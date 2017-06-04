using UnityEngine;
using Crab.Events;


namespace Crab
{
    public class SceneScript : MonoBehaviour
    {
        public string CustomGameInstance = "GameInstance";
        
        System.Type GameInstanceClass = typeof(GameInstance);

        // Static singleton property
        public static SceneScript Instance { get; private set; }

        void Awake()
        {
            if (CustomGameInstance.Length > 0 && CustomGameInstance != "GameInstance") {
                System.Type NewClass = System.Type.GetType(CustomGameInstance);
                if (NewClass != null)
                    GameInstanceClass = NewClass;
            }

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
        new public CameraMovement camera;

        void Start()
        {
            if (!FindObjectOfType<GameInstance>()) {
                //Create game Instance if no one is found
                GameObject gi = new GameObject("Game Instance", GameInstanceClass);
            }

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


        //Events
        protected virtual void BeforeGameStart() { }
        protected virtual void OnGameStart(PlayerController player) { }

        protected virtual void Update() { }
    }
}