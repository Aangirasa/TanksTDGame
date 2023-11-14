using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	[Export]
	public float movementSpeed;
	[Export]
	public float fireRate;


	private AnimatedSprite2D animatedSprite;
	private Area2D playerDetectorArea;
	private RayCast2D rayCast;
	private bool isPlayerInArea;
	private Node2D player;

	private float fireTime;
	private Vector2 movementVector;
	private Random random;
	private Vector2 targetVector;
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play(AnimationConstants.DEFAULT);
		animatedSprite.AnimationFinished += () => PlayDefaultAnimation();

		playerDetectorArea = GetNode<Area2D>("PlayerDetectorArea");
		playerDetectorArea.BodyEntered += (node) => PlayerEnteredTheScanRange(node);
		playerDetectorArea.BodyExited += (node) => PlayerExited(node);
		targetVector = Vector2.Zero;
		random = new Random();

		rayCast = GetNode<RayCast2D>("RayCast2D");
		rayCast.Enabled = true;

		this.BodyEntered += (node) =>
		{
			setNewTargetPointForPatrol();
		};
	}



	public override void _Process(double delta)
	{
		movementVector = Vector2.Zero;
		if (isPlayerInArea)
		{
			//persuit player and try to Fire
			if (player != null)
			{
				targetVector = Vector2.Zero;
				fireTime -= (float)delta;
				Vector2 direction = (player.GlobalPosition - this.GlobalPosition).Normalized();
				float degree = MathF.Atan2(direction.X, -direction.Y);
				this.Rotation = degree;

				movementVector = direction * movementSpeed * (float)delta;
				if (fireTime < 0)
				{
					fireTime = fireRate;
					//Fire
				}
			}
		}
		else
		{
			if (targetVector == Vector2.Zero
				|| targetVector.DistanceTo(this.GlobalPosition) < 2
				|| rayCast.IsColliding())
			{
				setNewTargetPointForPatrol();
			}

			// patrol the area 
			Vector2 direction = (targetVector - this.GlobalPosition).Normalized();
			float degree = MathF.Atan2(direction.X, -direction.Y);
			this.Rotation = degree;

			movementVector = direction * movementSpeed * (float)delta;
		}
	}

	private void setNewTargetPointForPatrol()
	{
		targetVector = GetRandomTargetPoint();
	}

	public override void _PhysicsProcess(double delta)
	{

		MoveAndCollide(movementVector);
	}
	private void PlayDefaultAnimation()
	{
		animatedSprite.Play(AnimationConstants.DEFAULT);
	}

	private void PlayerEnteredTheScanRange(Node2D node)
	{
		if (node is Tank)
		{
			player = node;
			isPlayerInArea = true;
		}

	}

	private Vector2 GetRandomTargetPoint()
	{
		float maxDistance = 70.0f;
		float randomAngle = (float)random.NextDouble() * Mathf.Pi * 2.0f;
		Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
		Vector2 targetPoint = GlobalPosition + direction * maxDistance;
		return targetPoint;
	}

	private void PlayerExited(Node2D node)
	{
		if (node is Tank)
		{
			player = null;
			isPlayerInArea = false;
		}

	}

	private class AnimationConstants
	{
		public static String DEFAULT = "default";
		public static String FIRE = "fire";
		public static String TAKE_HIT = "take_hit";
	}
}
