using UnityEngine;
using System.Collections;

public class ISpawn : Item {

    protected override void OnGameStart(SceneScript scene)
    {

    }

    public PlayerController Spawn(float height = 0) {
        if (!SceneScript.Instance.player)
        {
            Vector3 position = transform.position;
            position.y += height;
            return Instantiate(Cache.Get.player, position, transform.rotation) as PlayerController;
        }
        return null;
    }
}
