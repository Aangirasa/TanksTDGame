using Godot;
using System;

public partial class CameraShaker : Godot.Camera2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public float shakeDuration;
	[Export]
	public float intensity;
	private Vector2 originalPosition;
	private bool isShakeActive;
	private float shakeTime;
	private Random random = new Random();
	public static CameraShaker INSTANCE;
	public override void _Ready()
	{
		shakeTime = shakeDuration;
		INSTANCE = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isShakeActive)
		{
			shakeTime -= (float)delta;
			if(shakeTime < 0)
			{
				isShakeActive = false;
				this.Offset = originalPosition;
				shakeTime = shakeDuration;
				return;
			}
			this.Offset = originalPosition + new Vector2((float)random.NextDouble(), (float)random.NextDouble()) * intensity;
		}
	}

	public void shake()
	{
		originalPosition = this.Position;
		isShakeActive = true;
	}
}
