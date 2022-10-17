using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct TimeToLiveComponent : IComponentData
{
    /// <summary>
    /// How long the entity has been alive for
    /// </summary>
    public float Time;
}
