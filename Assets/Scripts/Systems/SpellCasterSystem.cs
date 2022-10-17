using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpellCasterSpawnerSystem : ISystem, ISystemStartStop
{
    Entity Player;
    LocalToWorld PlayerLTW;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
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
        PlayerLTW = SystemAPI.GetComponent<LocalToWorld>(Player);

        var spellCasterJob = new SpellCasterJob()
        {
            ecb = ecb,
            deltaTime = deltaTime,
            PlayerLTW = PlayerLTW
        };

        spellCasterJob.Schedule();
    }

    public void OnStartRunning(ref SystemState state)
    {
        Player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    public void OnStopRunning(ref SystemState state)
    {
    }
}

[BurstCompile]
public partial struct SpellCasterJob : IJobEntity
{
    public EntityCommandBuffer ecb;
    public float deltaTime;
    public LocalToWorld PlayerLTW;

    public void Execute(ref SpellCaster enemySpawner)
    {
        
        enemySpawner.CurrentCoolDown += deltaTime;
        if(enemySpawner.CurrentCoolDown >= enemySpawner.CoolDown)
        {
            enemySpawner.CurrentCoolDown = 0;
            var ent = ecb.Instantiate(enemySpawner.SpellPrefab);
            ecb.SetComponent(ent, new Translation { Value = PlayerLTW.Position });
        }
    }
}