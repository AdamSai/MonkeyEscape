using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// This system should run after the transform system has been updated, otherwise the camera
// will lag one frame behind the tank and will jitter.
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial class CameraSystem : SystemBase
{
    Entity Target;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<PlayerTag>();
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        Target = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    protected override void OnUpdate()
    {
        var cameraTransform = CameraSingleton.Instance.transform;
        var targetTransform = GetComponent<LocalToWorld>(Target);
        cameraTransform.position = targetTransform.Position - 10.0f * targetTransform.Forward + new float3(0.0f, 5.0f, 0.0f);
        cameraTransform.LookAt(targetTransform.Position, new float3(0.0f, 1.0f, 0.0f));
    }
}