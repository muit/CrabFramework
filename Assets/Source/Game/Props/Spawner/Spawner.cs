using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crab;
using Crab.Utils;

[System.Serializable]
public class SpawnPreset {
    [Tooltip("Entity that will be generated")]
    public Entity entityPrefab;
    public int spawnsPerMinute = 21;
    public int spawnVariation = 3;
    [Tooltip("Maximum amount of spawns")]
    public int maxSpawns = 100;
}

public class Spawner : MonoBehaviour
{

    public bool startActivated = false;
    
    [Tooltip("(Optional) Where the new entity will be added")]
    public GameObject containerObject;

    [Header("Entity")]
    public bool startCombatOnSpawn = true;

    public SpawnPreset defaultPreset;
    public List<SpawnPreset> levelPresets;

    private Delay spawnDelay;

    // Use this for initialization
    void Start () {
        spawnDelay = new Delay(GetSpawnLength(), startActivated);
	}

    public void Activate() {
        spawnDelay.Start(GetSpawnLength());
    }
	
	// Update is called once per frame
	void Update () {
        if (!spawnDelay.Over())
            return;

        //Start the event again
        spawnDelay.Start(GetSpawnLength());
        
        SpawnPreset preset = GetPreset();

        if (!preset.entityPrefab)
            return;

        Entity newEntity = Entity.Instantiate(preset.entityPrefab, transform.position, transform.rotation);

        if (containerObject) {
            newEntity.transform.parent = containerObject.transform;
        }

        if (startCombatOnSpawn) {
            //Start combat with player
            newEntity.ai.StartCombatWith(Cache.Get.player.Me);
        }
	}

    private int GetSpawnLength() {
        SpawnPreset preset = GetPreset();

        float finalSpawnsPerMinute = Mathf.Clamp(Random.Range(preset.spawnsPerMinute - preset.spawnVariation, preset.spawnsPerMinute + preset.spawnVariation), 1, 100000);
        return (int)(60/finalSpawnsPerMinute * 1000);
    }

    private SpawnPreset GetPreset()
    {
        SpawnPreset preset = null;

        AYSGameInstance gi = Cache.Get.gameInstance;
        if (gi != null && levelPresets.Count > gi.Level)
            preset = levelPresets[gi.Level];
        
        if (preset == null)
            return defaultPreset;

        return preset;
    }
}
