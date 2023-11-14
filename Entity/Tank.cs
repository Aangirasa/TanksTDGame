using Godot;
using System;

public partial class Tank : CharacterBody2D
{
	private AnimatedSprite2D animatedSprite;
	private RayCast2D rayCast;
	[Export]
	public PackedScene bulletScene;
	[Export]
	public float move_speed;
	private Vector2 input;


	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play("default");
		animatedSprite.AnimationFinished += () => animationFinished();
		rayCast = this.GetNode<RayCast2D>("RayCast2D");
	}

 

	public override void _Process(double delta)
	{
		 input = Input.GetVector("MOVE_LEFT", "MOVE_RIGHT", "MOVE_UP", "MOVE_DOWN").Normalized();
		if( !(input.X == 0 && input.Y == 0))
		{
			this.Rotation = GetRotation(input);
		}
		if (Input.IsActionJustPressed("FIRE"))
		{
			animatedSprite.Play("fire");
			RigidBody2D bulletBody = bulletScene.Instantiate() as RigidBody2D;
			bulletBody.Position = rayCast.GlobalPosition;
			bulletBody.Rotation = this.Rotation;
			float directionX = Mathf.Cos(this.Rotation - Mathf.Pi / 2);
			float directionY = Mathf.Sin(this.Rotation - Mathf.Pi / 2);
			var direction = new Vector2(directionX, directionY);
			bulletBody.LinearVelocity = direction * 500;
			GetParent().AddChild(bulletBody);
			CameraShaker.INSTANCE.shake();
		}
		
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndCollide((input * move_speed * (float)delta));
	}

	private float GetRotation(Vector2 input)
	{
		return Mathf.Atan2(input.X, -input.Y);

	}

	private void animationFinished()
	{
		animatedSprite.Play("default");
	}
}
