using UnityEngine;
using System.Collections;

public class GameStats : MonoBehaviour
{
    // Static singleton property
    private static GameStats instance;
    public static GameStats Get {
        get { 
            return instance ? instance : instance = FindObjectOfType<GameStats>();
        } 
        private set { instance = value; }
    }

    void Awake()
    {
        if (Get && Get != this) {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }
}