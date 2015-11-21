using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
    private EntityController controller;
    public EntityController GetController() { return controller; }


    private CMovement movement;
    public CMovement GetMovement() { return movement; }

    private CState state;
    public CState GetState() { return state; }

    private CAttributes attributes;
    public CAttributes GetAttributes() { return attributes; }

    private CInventory inventory;
    public CInventory GetInventory() { return inventory; }
    

    void Awake() {
        controller = GetComponent<EntityController>();

        movement = GetComponent<CMovement>();
        state = GetComponent<CState>();
        attributes = GetComponent<CAttributes>();
        inventory = GetComponent<CInventory>();
    }
}
