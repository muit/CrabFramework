using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Entity))]
[DisallowMultipleComponent]
public class CInventory : MonoBehaviour {
    private Entity me;
    void Awake() {
        me = GetComponent<Entity>();
    }
}
