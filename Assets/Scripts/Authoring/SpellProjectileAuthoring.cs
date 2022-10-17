using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class SpellProjectileAuthoring : MonoBehaviour
{
    public float Speed;
    public float AngularVelocity;
    public float3 Direction;
    public float3 Position;
    public float TimeToLive;
}

public class SpellProjectileBaker : Baker<SpellProjectileAuthoring>
{
    public override void Bake(SpellProjectileAuthoring authoring)
    {
        AddComponent(new SpellProjectile
        {
            Speed = authoring.Speed,
            AngularVelocity = authoring.AngularVelocity,
            Direction = authoring.Direction,
            Position = authoring.Position
        });
        // If no time to live value has been set, use the default value of 10 seconds
        AddComponent(new TimeToLiveComponent { Time = authoring.TimeToLive == 0f ? 10f : authoring.TimeToLive });
    }
}
