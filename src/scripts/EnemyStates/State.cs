using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class State
{
	public abstract void process(float delta, Node2D Player);
	public abstract bool isValidTransition(EnemyStates nextState);
}

