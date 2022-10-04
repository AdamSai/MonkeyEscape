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
        var delta = UnityEngine.Time.deltaTime;

        Entities.ForEach((ref Position pos, ref PhysicsVelocity velocity, in PlayerTag tag,
            in MoveComponent moveComponent, in LocalToWorld ltw) =>
        {
            var move = new float3(horizontal, vertical, 0);
            pos.Value += move * moveComponent.Speed * delta;
            velocity.Linear = new float3(horizontal, vertical, 0) * moveComponent.Speed;
        }).WithBurst().ScheduleParallel();
    }
}