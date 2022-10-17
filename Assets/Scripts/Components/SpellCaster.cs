using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct SpellCaster : IComponentData
{
    public float CoolDown;
    public float CurrentCoolDown;
    public Entity SpellPrefab;
}
