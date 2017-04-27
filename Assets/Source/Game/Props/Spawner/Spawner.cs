using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;
using Crab.Utils;

public class Spawner : MonoBehaviour
{

    [Tooltip("Entity that will be generated")]
    public Entity entityPrefab;
    [Tooltip("(Optional) Where the new entity will be added")]
    public GameObject containerObject;
    public int spawnsPerMinute = 21;
    public int spawnVariation = 3;
    [Tooltip("Maximum amount of spawns")]
    public int maxSpawns = 100;

    [Header("Entity")]
    public bool startCombatOnSpawn = true;

    private Delay spawnDelay;

    // Use this for initialization
    void Start () {
        spawnDelay = new Delay(GetSpawnLength(), true);
	}
	
	// Update is called once per frame
	void Update () {
        if (spawnDelay.Over()) {

            //Start the event again
            spawnDelay.Start(GetSpawnLength());

            if (!entityPrefab)
                return;

            Entity newEntity = Entity.Instantiate(entityPrefab, transform.position, transform.rotation);

            if (containerObject) {
                newEntity.transform.parent = containerObject.transform;
            }

            if (startCombatOnSpawn) {
                //Start combat with player
                newEntity.ai.StartCombatWith(Cache.Get.player.Me);
            }

        }
	}

    private int GetSpawnLength() {
        float finalSpawnsPerMinute = Mathf.Clamp(Random.Range(spawnsPerMinute - spawnVariation, spawnsPerMinute + spawnVariation), 1, 100000);
        return (int)(60/finalSpawnsPerMinute * 1000);
    }
}
