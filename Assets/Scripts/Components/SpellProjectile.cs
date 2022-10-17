using Unity.Entities;
using Unity.Mathematics;

public struct SpellProjectile : IComponentData
{
    public float Speed;
    public float AngularVelocity;
    public float3 Direction;
    public float3 Position;
}
