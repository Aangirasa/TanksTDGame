using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PatrollingState : State
{
	private Vector2 targetVector;
	private Vector2 movementVector;
	private float movementSpeed;
	private Random random;
	private AnimatedSprite2D enemyAnimatedSprite;
	private RigidBody2D enemy;
	private RayCast2D farwardRayCast;

	public PatrollingState(float movementSpeed, RigidBody2D enemy, RayCast2D farwardRayCast, AnimatedSprite2D animatedSprite)
	{
		this.movementSpeed = movementSpeed;
		this.enemy = enemy; 
		this.farwardRayCast = farwardRayCast;
		this.enemyAnimatedSprite = animatedSprite;
		targetVector = Vector2.Zero; 
		movementVector = Vector2.Zero;
		random = new Random();
	}
	public override void process(float delta, Node2D Player)
	{
		if (targetVector == Vector2.Zero
				|| targetVector.DistanceTo(enemy.GlobalPosition) < 2
				|| farwardRayCast.IsColliding())
		{
			setNewTargetPointForPatrol();
		}

		// patrol the area 
		Vector2 direction = (targetVector - enemy.GlobalPosition).Normalized();
		float degree = MathF.Atan2(direction.X, -direction.Y);
		enemy.Rotation = degree;

		movementVector = direction * movementSpeed * (float)delta;
		enemy.MoveAndCollide(movementVector);
	}

	private void setNewTargetPointForPatrol()
	{
		targetVector = GetRandomTargetPoint();
	}

	private Vector2 GetRandomTargetPoint()
	{
		float maxDistance = 70.0f;
		float randomAngle = (float)random.NextDouble() * Mathf.Pi * 2.0f;
		Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
		Vector2 targetPoint = enemy.GlobalPosition + direction * maxDistance;
		return targetPoint;
	}

	public override bool isValidTransition(EnemyStates nextState)
	{
		return nextState.Equals(EnemyStates.PersuitState);
	}
}

