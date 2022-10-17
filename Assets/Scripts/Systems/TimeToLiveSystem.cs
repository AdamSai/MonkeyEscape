using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [BurstCompile]
    public partial struct TimeToLiveSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            var deltaTime = Time.deltaTime;

            var ttlJob = new TimeToLiveJob
            {
                ecb = ecb,
                DeltaTime = deltaTime
            };
            
            ttlJob.Run();
        }
    }

    [BurstCompile]
    public partial struct TimeToLiveJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ecb;
        public void Execute(in Entity entity, ref TimeToLiveComponent timeToLive)
        {
            timeToLive.Time -= DeltaTime;
            if (timeToLive.Time <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
}