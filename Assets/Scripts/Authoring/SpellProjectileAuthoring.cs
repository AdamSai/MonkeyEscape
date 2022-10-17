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
    }
}
