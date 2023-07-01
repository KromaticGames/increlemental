using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Increlemental.Entities;

public partial class PawnCollector : ModelEntity
{
    [Net]
    public float Radius { get; set; } = 50f;
    private float OldRadius;

    public AreaUI WorldUI { get; set; }

    [Net]
    public Pawn Pawn { get; set; }

    public override void Spawn() {
        base.Spawn();
        OldRadius = Radius;

        Name = "Pawn Collector";
        SetupPhysicsFromCylinder( PhysicsMotionType.Static, new Capsule(Vector3.Zero, Vector3.Up * 60f, Radius));
        
        Transmit = TransmitType.Always;

        Tags.Add("pawncollector");
    }

    [GameEvent.Tick]
    public void Tick()
    {
        if (OldRadius != Radius)
            SetupPhysicsFromCylinder( PhysicsMotionType.Static, new Capsule(Vector3.Zero, Vector3.Up * 60f, Radius));
        
        OldRadius = Radius;
    }

    public override void ClientSpawn()
    {
        base.ClientSpawn();

        float AreaSize = 1000 * Radius/25;
        WorldUI = new()
        {
            Transform = Transform,
            Position = Position + Vector3.Up * .1f,
            Rotation = Rotation.FromPitch(90f),
            WorldScale = Scale, //* Radius/25;
            PanelBounds = new Rect(-AreaSize/2, -AreaSize/2, AreaSize, AreaSize)
        };
    }

    [GameEvent.Client.Frame]
    public void FrameTick()
    {
        WorldUI.Position = Position + Vector3.Up * .1f;

        float AreaSize = 1000 * Radius/25;
        WorldUI.PanelBounds = new Rect(-AreaSize/2, -AreaSize/2, AreaSize, AreaSize);
    }
}