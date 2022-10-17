using Components;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [BurstCompile]
    public partial struct HealthSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            // var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            // new HealthSystemJob{ecb = ecb}.Run();
        }
    }

    [BurstCompile]
    public partial struct HealthSystemJob : IJobEntity
    {
        public EntityCommandBuffer ecb;

        public void Execute(Entity entity, in HealthComponent healthComponent)
        {
            if (healthComponent.Value <= 0f)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
}