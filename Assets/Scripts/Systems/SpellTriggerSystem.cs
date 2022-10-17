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
    
            var hp = new HealthSystemJob{ecb = ecb}.Schedule(state.Dependency);

            var dependency = JobHandle.CombineDependencies(hp, state.Dependency);
            state.Dependency = new SpellTriggerJob
            {
                EnemyTagData = m_ComponentDataHandles.EnemyTagLookUp,
                HealthComponentData = m_ComponentDataHandles.HealthComponentLookup
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), dependency);
            

        }
    }

    [BurstCompile]
    struct SpellTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<EnemyTag> EnemyTagData;
        public ComponentLookup<HealthComponent> HealthComponentData;

        public void Execute(TriggerEvent collisionEvent)
        {
            Entity entityB = collisionEvent.EntityB;

            bool isBodyAEnemy = EnemyTagData.HasComponent(entityB);
            bool hasHealthComponent = HealthComponentData.HasComponent(entityB);


            if (isBodyAEnemy && hasHealthComponent)
            {
                Debug.Log("Bah");
                var healthComponent = HealthComponentData.GetRefRW(entityB, false);
                healthComponent.ValueRW.Value -= 10;
                Debug.Log("Health: " + healthComponent.ValueRO.Value);
            }
        }
    }
}