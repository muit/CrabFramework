using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    void Start()
    {
        OnGameStart(SceneScript.Instance);
    }

    public virtual void Reset()
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    //Events
    protected virtual void OnGameStart(SceneScript scene) { }
}
