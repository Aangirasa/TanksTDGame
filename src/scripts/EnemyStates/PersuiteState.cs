using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PersuiteState : State
{
	private RigidBody2D enemy;
	private float fireRate;
	private float fireTime;
	private Vector2 movementVector;
	private float movementSpeed;
	private AnimatedSprite2D animatedSprite;
	private PackedScene bulletScene;
	private RayCast2D rayCast;

	public PersuiteState(RigidBody2D enemy, float fireRate, float movementSpeed, AnimatedSprite2D animatedSprite, PackedScene bulletScene, RayCast2D rayCast)
	{
		this.enemy = enemy;
		this.fireRate = fireRate;
		this.fireTime = 0;
		this.movementSpeed = movementSpeed;
		this.animatedSprite = animatedSprite;
		this.bulletScene = bulletScene;
		this.rayCast = rayCast;
	}

	public override void process(float delta, Node2D player)
	{
		if (player != null)
		{

			fireTime -= (float)delta;
			Vector2 direction = (player.GlobalPosition - enemy.GlobalPosition).Normalized();
			float degree = MathF.Atan2(direction.X, -direction.Y);
			enemy.Rotation = degree;

			movementVector = direction * movementSpeed * (float)delta;
			if (fireTime < 0)
			{
				fireTime = fireRate;
				FireAtPlayer();
			}
			enemy.MoveAndCollide(movementVector);
		}
	}

	public void FireAtPlayer()
	{
		animatedSprite.Play(AnimationConstants.FIRE);
		RigidBody2D bulletBody = bulletScene.Instantiate() as RigidBody2D;
		bulletBody.Position = rayCast.GlobalPosition;
		bulletBody.Rotation = enemy.Rotation;
		float directionX = Mathf.Cos(enemy.Rotation - Mathf.Pi / 2);
		float directionY = Mathf.Sin(enemy.Rotation - Mathf.Pi / 2);
		var direction = new Vector2(directionX, directionY);
		bulletBody.LinearVelocity = direction * 500;
		enemy.GetParent().AddChild(bulletBody);
	}

	public override bool isValidTransition(EnemyStates nextState)
	{
		return nextState.Equals(EnemyStates.PatrollingState);
	}
}

