using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster[] monsterPrefabs = new Monster[0];
    public Transform[] spawnLocations = new Transform[0];

    public uint targetMonsterCount;
    public float minimumTimeBetweenSpawns;

    private List<Monster> monsters = new List<Monster>();
    private float lastSpawnTime;

    private void Start()
    {
        if (spawnLocations.Length == 0)
        {
            spawnLocations = new Transform[1];
            spawnLocations[0] = transform;
        }
    }

    private void Update()
    {
        if (monsters.Count < targetMonsterCount &&
            lastSpawnTime + minimumTimeBetweenSpawns < Time.time)
        {
            lastSpawnTime = Time.time;

            TrySpawnMonster();
        }
    }

    public void KillMonster(Monster monster)
    {
        monsters.Remove(monster);
        Destroy(monster.gameObject);

        GetComponentInParent<GameControl>().KillMonster();
    }

    private void TrySpawnMonster()
    {
        if (monsterPrefabs.Length == 0 || spawnLocations.Length == 0)
            return;

        int monsterIndex = UnityEngine.Random.Range(0, monsterPrefabs.Length);
        int spawnLocationIndex = UnityEngine.Random.Range(0, spawnLocations.Length);

        var monsterPrefab = monsterPrefabs[monsterIndex];
        var spawnLocation = spawnLocations[spawnLocationIndex];

        var monster = Instantiate(monsterPrefab, spawnLocation.position, spawnLocation.rotation, transform);
        monsters.Add(monster);
    }
}
