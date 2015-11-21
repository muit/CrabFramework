using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Entity))]
[DisallowMultipleComponent]


public class CState : MonoBehaviour {
    public enum CombatState
    {
        Default,
        Combat
    }

    private Entity me;
    void Awake()
    {
        me = GetComponent<Entity>();

        combat = CombatState.Default;
    }

    public CombatState combat;
}
