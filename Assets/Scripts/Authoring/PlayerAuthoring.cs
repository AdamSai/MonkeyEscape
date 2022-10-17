using Components;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float speed;
    public float health;
}

public class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        AddComponent(new MoveComponent { Speed = authoring.speed });
        AddComponent(new PlayerTag());
        AddComponent(new HealthComponent {Value = authoring.health });
    }
}