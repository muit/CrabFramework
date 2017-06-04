using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crab
{
    public class GameInstance : MonoBehaviour
    {
        // Static singleton property
        public static GameInstance Instance { get; private set; }

        void Awake()
        {
            // Check if there are any other instances conflicting
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            BeforeConstruct();
            Construct();
        }


        //Events
        protected virtual void BeforeConstruct() { }
        protected virtual void Construct() { }

        protected virtual void Update() { }



        static public void LoadScene(int scene)
        {
            SceneManager.LoadSceneAsync(scene);
        }

        static public void LoadScene(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
        }

        /*protected static virtual void RegistryFactory() {
            
        }*/
    }
}
