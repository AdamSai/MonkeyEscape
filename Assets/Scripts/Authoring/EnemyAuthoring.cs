using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float speed; 
    public float health; 

}

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        AddComponent(new EnemyTag());
        AddComponent(new MoveComponent {Speed = authoring.speed});
        AddComponent(new HealthComponent {Value = authoring.health });
    }   
}