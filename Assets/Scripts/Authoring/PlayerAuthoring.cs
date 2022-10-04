using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float speed;
}

public class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        AddComponent(new MoveComponent { Speed = authoring.speed });
        AddComponent(new PlayerTag());
        AddComponent(new Position { Value = GetComponent<Transform>().position });
    }
}