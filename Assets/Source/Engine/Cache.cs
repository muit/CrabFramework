using UnityEngine;
using System.Collections;

public class Cache : MonoBehaviour {
    [Header("Character Prefabs")]
    public PlayerController player;

    [Header("Object Prefabs")]


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
