using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Increlemental.Entities;

public class PawnCollector : ModelEntity
{
    public float Radius { get; set; } = 25f;
    public AreaUI WorldUI { get; set; }

    public Pawn Pawn { get; set; }


    public override void Spawn() {
        base.Spawn();

        Name = "Pawn Collector";
        SetupPhysicsFromCylinder( PhysicsMotionType.Static, new Capsule(Vector3.Zero, Vector3.Up * 60f, Radius));
        
        Transmit = TransmitType.Always;

        Tags.Add("pawncollector");
    }

    public override void ClientSpawn()
    {
        base.ClientSpawn();

        float areaRadius = Radius * 10 + 200;
        WorldUI = new()
        {
            Transform = Transform,
            Position = Position + Vector3.Up * 5.5f,
            Rotation = Rotation.FromPitch(90f),
            PanelBounds = new Rect(-areaRadius / 2, -areaRadius / 2, areaRadius, areaRadius),
            WorldScale = Scale * 1.3f,
            
        };
    }
}