using System.Numerics;
using Sandbox;
using System;

namespace Increlemental.Entities;

public partial class GeneratorArea : BaseTrigger
{
    public float Radius { get; set; } = 400f;

    public override void Spawn()
    {
        SetupPhysicsFromCylinder( PhysicsMotionType.Static, new Capsule(Vector3.Zero, Vector3.Up * 50, Radius));

        EnableAllCollisions = false;
        EnableTouch = true;

        ActivationTags.Add("coin");
        Tags.Add("genarea");
    }
}