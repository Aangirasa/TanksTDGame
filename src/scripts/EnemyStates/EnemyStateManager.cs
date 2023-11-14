using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EnemyStateManager
{
	private PatrollingState patrollingState;
	private PersuiteState persuiteState;
	private State currentState;

	public EnemyStateManager(RigidBody2D enemy, float movementSpeed, float fireRate, AnimatedSprite2D animatedSprite, RayCast2D farwardRayCast, PackedScene bulletScene)
	{
		patrollingState = new PatrollingState(movementSpeed, enemy, farwardRayCast, animatedSprite);
		persuiteState = new PersuiteState(enemy, fireRate, movementSpeed, animatedSprite, bulletScene, farwardRayCast);
		currentState = patrollingState;
	}

	public void process(float delta, Node2D Player)
	{
		currentState.process(delta, Player);
	}

	public void transitionToState(EnemyStates nextState)
	{
		if (currentState.isValidTransition(nextState))
		{
			currentState = getNextState(nextState);
		}
	}

	private State getNextState(EnemyStates nextState)
	{
		switch (nextState)
		{
			case EnemyStates.PatrollingState: return patrollingState;
			case EnemyStates.PersuitState: return persuiteState;
			default: return currentState;
		}
	}
}

