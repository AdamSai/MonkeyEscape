using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class SpellCasterAuthoring : MonoBehaviour
{
    public float CoolDown;
    public float CurrentCoolDown;
    public GameObject SpellPrefab;
}

public class SpellCasterBaker : Baker<SpellCasterAuthoring>
{
    public override void Bake(SpellCasterAuthoring authoring)
    {
        AddComponent(new SpellCaster
        {
            CoolDown = authoring.CoolDown,
            CurrentCoolDown = authoring.CurrentCoolDown,
            SpellPrefab = GetEntity(authoring.SpellPrefab)
        });
    }
}