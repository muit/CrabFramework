﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CMovement))]
public class PlayerController : EntityController {
    private Entity me;
    private CMovement movement;
    void Awake() {
        me = GetComponent<Entity>();
        movement = me.GetMovement();
    }

    void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        movement.Move(new Vector3(h, 0, v));

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            movement.StartSprint();
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift)) {
            movement.StopSprint();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            movement.StartCrouching();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl)) {
            movement.StopCrouching();
        }
    }
}
