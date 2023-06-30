using Sandbox;
using System;

namespace Increlemental.Entities;

[Library( "inc_coin", Title = "Coin" )]
public partial class Coin : ModelEntity
{
	public Vector3 PositionOffset { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		Name = "Coin";
        SetModel("models/citizen_props/coin01.vmdl_c");
		SetupPhysicsFromSphere( PhysicsMotionType.Static, Vector3.Forward * -15, 15f );

		EnableAllCollisions = true;
		EnableSolidCollisions = true;
        EnableTouch = true;

        Rotation = Rotation.FromPitch(90f) * Rotation.FromRoll( Time.Now * 100f );

		Tags.Add( "coin" );

		Transmit = TransmitType.Always;
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );

		if (other is PawnCollector c)
		{
			Log.Info(c.Pawn);
			c.Pawn.Resources.Coins += 1;

			if ( Game.IsServer )
			{
				Delete();
			}
		}
    }

	[GameEvent.Client.Frame]
	public void Frame()
	{
		SceneObject.Rotation = Rotation.FromPitch( 90f ) * Rotation.FromRoll(Time.Now * 100f);
		PositionOffset = Vector3.Up * ((float)Math.Sin( Time.Now ) * 5);
		SceneObject.Position = Position + PositionOffset;
	}
}
