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
    public float minimumDistanceToPlayer;

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

    private uint currentMonsterIndex = 0;

    private void FixedUpdate()
    {
        if (monsters.Count == 0)
            return;

        var monstersUpdated = 0;
        var startTime = Time.realtimeSinceStartup;
        do
        {
            monsters[(int)(currentMonsterIndex % monsters.Count)].OnAI();

            ++currentMonsterIndex;
            ++monstersUpdated;
        }
        while (monstersUpdated < monsters.Count && Time.realtimeSinceStartup - startTime < 0.005);
    }

    public void KillMonster(Monster monster)
    {
        Instantiate(monster.deathPrefab, monster.transform.position, monster.transform.rotation, transform);

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

        var player = FindObjectOfType<Player>();
        if (Vector3.Distance(player.transform.position, spawnLocation.transform.position) < minimumDistanceToPlayer)
            return;

        var monster = Instantiate(monsterPrefab, spawnLocation.position, spawnLocation.rotation, transform);
        monsters.Add(monster);
    }
}
