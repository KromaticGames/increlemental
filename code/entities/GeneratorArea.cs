using System.Numerics;
using Sandbox;
using System;

namespace Increlemental.Entities;

public partial class GeneratorArea : BaseTrigger
{
    public float Radius { get; set; } = 400f;
    AreaUI WorldUI;

    public override void Spawn()
    {
        SetupPhysicsFromCylinder( PhysicsMotionType.Static, new Capsule(Vector3.Zero, Vector3.Up * 50, Radius));

        EnableAllCollisions = false;
        EnableTouch = true;

        ActivationTags.Add("coin");
        Tags.Add("genarea");
    }
    public override void ClientSpawn()
    {
        base.ClientSpawn();

        float AreaSize = 1000 * Radius/25;
        WorldUI = new()
        {
            Transform = Transform,
            Position = Position + Vector3.Up * 5f,
            Rotation = Rotation.FromPitch(90f),
            WorldScale = Scale, //* Radius/25;
            PanelBounds = new Rect(-AreaSize/2, -AreaSize/2, AreaSize, AreaSize)
        };
    }

    [GameEvent.Client.Frame]
    public void FrameTick()
    {
        float AreaSize = 1000 * Radius/25;
        WorldUI.PanelBounds = new Rect(-AreaSize/2, -AreaSize/2, AreaSize, AreaSize);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        WorldUI?.Delete();
    }
}