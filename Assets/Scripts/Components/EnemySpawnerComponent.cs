using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct EnemySpawnerComponent : IComponentData
{
    public Entity enemyPrefab;
    public float SpawnRate;
    public float SpawnTimer;
}
