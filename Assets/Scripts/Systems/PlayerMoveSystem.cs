using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial class PlayerMoveSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        
    }

    protected override void OnUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Entities.ForEach((ref PhysicsVelocity velocity, in PlayerTag tag,
            in MoveComponent moveComponent, in LocalToWorld ltw) =>
        {
            velocity.Linear = new float3(horizontal, vertical, 0) * moveComponent.Speed;
        }).WithBurst().ScheduleParallel();
    }
}