using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 1f;
}


public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
{
    public override void Bake(EnemySpawnerAuthoring authoring)
    {
        AddComponent(new EnemySpawnerComponent
        {
            enemyPrefab =  GetEntity(authoring.enemyPrefab),
            SpawnRate = authoring.spawnRate,
            SpawnTimer = 0f
        });
    }
}