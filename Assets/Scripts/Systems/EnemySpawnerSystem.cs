using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemySpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnDestroy(ref SystemState state)
    {
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Creating an EntityCommandBuffer to defer the structural changes required by instantiation.
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var deltaTime = Time.deltaTime;

        var enemySpawnerJob = new EnemySpawnerJob
        {
            ecb = ecb,
            deltaTime = deltaTime
        };

        enemySpawnerJob.Schedule();
    }
}

[BurstCompile]
public partial struct EnemySpawnerJob : IJobEntity
{
    public EntityCommandBuffer ecb;
    public float deltaTime;
    public void Execute(ref EnemySpawnerComponent enemySpawner, in Translation ltw)
    {
        enemySpawner.SpawnTimer += deltaTime;
        if(enemySpawner.SpawnTimer >= enemySpawner.SpawnRate)
        {
            enemySpawner.SpawnTimer = 0;
            var ent = ecb.Instantiate(enemySpawner.enemyPrefab);
            ecb.SetComponent(ent, new Translation { Value = ltw.Value });
        }
    }
}