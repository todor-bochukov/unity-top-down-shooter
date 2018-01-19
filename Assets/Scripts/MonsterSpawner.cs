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

    private GameControl control;

    private List<Monster> monsters = new List<Monster>();
    private float lastSpawnTime;

    private void Start()
    {
        control = GetComponentInParent<GameControl>();

        if (spawnLocations.Length == 0)
        {
            spawnLocations = new Transform[1];
            spawnLocations[0] = transform;
        }
    }

    private void Update()
    {
        if (monsters.Count < targetMonsterCount &&
            lastSpawnTime + minimumTimeBetweenSpawns < Time.time * control.TimeScale)
        {
            lastSpawnTime = Time.time;

            TrySpawnMonster();
        }
    }

    private void OnEnable()
    {
        var control = GetComponentInParent<GameControl>();
        if (control != null)
            control.onRestart += Restart;
    }

    private void OnDisable()
    {
        var control = GetComponentInParent<GameControl>();
        if (control != null)
            control.onRestart += Restart;
    }

    public void KillMonster(Monster monster)
    {
        monsters.Remove(monster);
        Destroy(monster.gameObject);
    }

    private void Restart()
    {
        foreach (var monster in monsters)
        {
            Destroy(monster.gameObject);
        }
        monsters.Clear();

        lastSpawnTime = Time.time;
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
