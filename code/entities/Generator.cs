using Sandbox;
using System;

namespace Increlemental.Entities;

[Library( "inc_generator", Title = "Generator" )]
public partial class Generator : ModelEntity
{
    [Net]
    public double NextSpawnDelay { get; set; } = 2;

    [Net]
    public double NextSpawnTimer { get; set; } = 0;

    [Net]
    public int MaxCoins { get; set; } = 10;

    [Net]
    public float Radius { get; set; } = 400f;

    Random Rand;

    GeneratorArea Area;

    AreaUI WorldUI;

    public override void Spawn()
    {
        base.Spawn();

		Name = "Generator";
        SetModel("models/oildrum.vmdl_c");
        Scale = 3f;

		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableAllCollisions = true;
        
		Transmit = TransmitType.Always;

		Rand = new();
        Area = new();

        Area.SetParent(this);
    }



    [GameEvent.Tick.Server]
    public void ServerTick()
    {
        NextSpawnTimer += Time.Delta;
        DebugOverlay.Text($"{Area.TouchingEntityCount}, {NextSpawnDelay-NextSpawnTimer}", Position);
        if (NextSpawnTimer > NextSpawnDelay)
        {
            NextSpawnDelay = 1 + Rand.NextDouble() * 3;
            NextSpawnTimer = 0;

            if (Area.TouchingEntityCount < MaxCoins)
            {
                Coin coin = new()
                {
                    Position = Position + Vector3.Up * 10 + (Rotation.FromYaw( Rand.Next(0, 360) ).Forward * (60 + Rand.Next(0, (int)Math.Floor(Radius) - 60)))
                };
            }
		}
    }

    protected override void OnDestroy() {
        base.OnDestroy();
    }
}