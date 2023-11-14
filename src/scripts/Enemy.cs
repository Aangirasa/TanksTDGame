using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	[Export]
	public float movementSpeed;
	[Export]
	public float fireRate;
	[Export]
	public PackedScene bulletScene;


	private AnimatedSprite2D animatedSprite;
	private Area2D playerDetectorArea;
	private RayCast2D rayCast;
	private bool isPlayerInArea;
	private Node2D player;

	private float fireTime;
	private Vector2 movementVector;
	private Random random;

	private EnemyStateManager enemyStateManager;
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play(AnimationConstants.DEFAULT);
		animatedSprite.AnimationFinished += () => PlayDefaultAnimation();

		playerDetectorArea = GetNode<Area2D>("PlayerDetectorArea");
		playerDetectorArea.BodyEntered += (node) => PlayerEnteredTheScanRange(node);
		playerDetectorArea.BodyExited += (node) => PlayerExited(node);

		random = new Random();

		rayCast = GetNode<RayCast2D>("RayCast2D");
		rayCast.Enabled = true;

		this.BodyEntered += (node) =>
		{
			this.Rotate(59);
		};
		enemyStateManager = new EnemyStateManager(this, movementSpeed, fireRate, animatedSprite, rayCast, bulletScene);
	}



	public override void _Process(double delta)
	{

	}


	public override void _PhysicsProcess(double delta)
	{
		movementVector = Vector2.Zero;
		enemyStateManager.process((float)delta, player);
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
			enemyStateManager.transitionToState(EnemyStates.PersuitState);
		}

	}

	private void PlayerExited(Node2D node)
	{
		if (node is Tank)
		{
			player = null;
			isPlayerInArea = false;
			enemyStateManager.transitionToState(EnemyStates.PatrollingState);
		}

	}
}
