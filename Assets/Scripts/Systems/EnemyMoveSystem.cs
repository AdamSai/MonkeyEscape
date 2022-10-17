using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial class EnemyMoveSystem : SystemBase
{
    Entity Target;
    LocalToWorld TargetLocalToWorld;
    
    [BurstCompile]
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        Target = SystemAPI.GetSingletonEntity<PlayerTag>();
    }
    
    [BurstCompile]
    protected override void OnUpdate()
    {
        TargetLocalToWorld = SystemAPI.GetComponent<LocalToWorld>(Target);
        new FollowPlayerJob
        {
            TargetLocalToWorld = TargetLocalToWorld
        }.Schedule();
    }
}

[BurstCompile]
public partial struct FollowPlayerJob : IJobEntity
{
    public LocalToWorld TargetLocalToWorld;
    
    public void Execute(in EnemyTag tag, in MoveComponent moveComponent, ref LocalToWorld ltw, ref PhysicsVelocity velocity)
    {
        var distance = math.distancesq(TargetLocalToWorld.Position, ltw.Position);
        if (distance > 5f)
            velocity.Linear = math.normalize(TargetLocalToWorld.Position - ltw.Position) * moveComponent.Speed;
        else
            velocity.Linear = float3.zero;
    }
}

