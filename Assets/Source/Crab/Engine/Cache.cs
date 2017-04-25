using UnityEngine;
using Crab;

public class Cache : MonoBehaviour {
    [Header("Character Prefabs")]
    public PlayerController player;

    //[Header("Object Prefabs")]


    [Header("References")]
    public new Camera camera;

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
