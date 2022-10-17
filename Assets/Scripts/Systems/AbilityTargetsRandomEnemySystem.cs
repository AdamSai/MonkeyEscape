using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

[BurstCompile]
public partial struct AbilityTargetsRandomEnemySystem : ISystem, ISystemStartStop
{
    Entity Player;
    LocalToWorld PlayerLTW;
    LocalToWorld EnemyLTW;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnStartRunning(ref SystemState state)
    {
        Player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    public void OnStopRunning(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
            EntityQuery query = new EntityQueryBuilder(Allocator.Temp)
                .WithAllRW<EnemyTag>()
                .WithAll<LocalToWorld>()
                .Build(state.EntityManager);
            var entities = query.ToEntityArray(Allocator.Temp);
            if (entities.Length > 0)
            {
                var rand = Random.Range(0, entities.Length);
                EnemyLTW = state.EntityManager.GetComponentData<LocalToWorld>(entities[rand]);
            }
            else
            {
                return;
            }
            
            PlayerLTW = SystemAPI.GetComponent<LocalToWorld>(Player);
            new AbilityTargetsRandomEnemyJob
            {
                PlayerLTW = PlayerLTW,
                EnemyLTW = EnemyLTW
            }.Schedule();
    }
}

[BurstCompile]
public partial struct AbilityTargetsRandomEnemyJob : IJobEntity
{
    public LocalToWorld PlayerLTW;
    public LocalToWorld EnemyLTW;

    public void Execute(ref SpellProjectile spell, ref PhysicsVelocity velocity)
    {

        if (spell.Direction.Equals(default))
        {
            var dir = math.normalize(EnemyLTW.Position - PlayerLTW.Position);
            spell.Direction = dir;
        }
        
        velocity.Linear = spell.Direction * spell.Speed;
    }
}