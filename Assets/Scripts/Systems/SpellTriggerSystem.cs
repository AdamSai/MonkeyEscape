using Components;
using Sirenix.OdinInspector;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace Systems
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [BurstCompile]
    public partial struct SpellTriggerSystem : ISystem
    {
        internal ComponentDataHandles m_ComponentDataHandles;

        internal struct ComponentDataHandles
        {
            public ComponentLookup<HealthComponent> HealthComponentLookup;
            public ComponentLookup<EnemyTag> EnemyTagLookUp;

            public ComponentDataHandles(ref SystemState systemState)
            {
                EnemyTagLookUp = systemState.GetComponentLookup<EnemyTag>(false);
                HealthComponentLookup = systemState.GetComponentLookup<HealthComponent>(false);
            }

            public void Update(ref SystemState systemState)
            {
                EnemyTagLookUp.Update(ref systemState);
                HealthComponentLookup.Update(ref systemState);
            }
        }

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate(state.GetEntityQuery(ComponentType.ReadOnly<EnemyTag>()));
            m_ComponentDataHandles = new ComponentDataHandles(ref state);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            m_ComponentDataHandles.Update(ref state);
    
            new HealthSystemJob{ecb = ecb}.Schedule(state.Dependency);

            state.Dependency = new SpellTriggerJob
            {
                EnemyTagData = m_ComponentDataHandles.EnemyTagLookUp,
                HealthComponentData = m_ComponentDataHandles.HealthComponentLookup
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        }
    }

    [BurstCompile]
    public partial struct SpellTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<EnemyTag> EnemyTagData;
        public ComponentLookup<HealthComponent> HealthComponentData;

        public void Execute(TriggerEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isBodyAEnemy = EnemyTagData.HasComponent(entityA);
            bool isBodyBEnemy = EnemyTagData.HasComponent(entityB);
            bool bodyAHasHealthComponent = HealthComponentData.HasComponent(entityA);
            bool bodyBHasHealthComponent = HealthComponentData.HasComponent(entityB);

            if (isBodyAEnemy && bodyAHasHealthComponent)
            {
                var healthComponent = HealthComponentData.GetRefRW(entityA, false);
                healthComponent.ValueRW.Value -= 10;
            }
            else if (isBodyBEnemy && bodyBHasHealthComponent)
            {
                var healthComponent = HealthComponentData.GetRefRW(entityB, false);
                healthComponent.ValueRW.Value -= 10;
            }
        }
    }
}