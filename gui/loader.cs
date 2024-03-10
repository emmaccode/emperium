using Godot;
using System;
// The goal of the loader is to handle everything from players and their inputs
// to map-loading, and controls :)
public partial class loader : Control
{
	public override void _Ready()
	{
	}
	public void LoadTo(string path)
	{
		PackedScene loadedScene = ResourceLoader.Load<PackedScene>(path);
		GetTree().Root.AddChild(loadedScene.Instantiate());
	}
}

public partial class Player : CharacterBody3D
{
	public const float Speed = 10.0f;
	public const float JumpVelocity = 8.5f;
	public bool Locked = false;
	public bool Flipped = false;
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	// process on each frame, calls `Platform` or `Fight`, then animates and moves.         
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		if (Input.IsActionJustPressed("lock") && IsOnFloor()) {
			Locked = !(Locked);
		}
		if (Locked) {
			velocity = Fight(velocity, delta);
		}
		else 
		{
			velocity = Platform(velocity, delta);
		}
		Velocity = velocity;
		MoveAndSlide();
		Animate();
	}
	public Vector3 Platform(Vector3 velocity, double delta){
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
			if (direction.X < 0) {
				Flipped = true;
			}
			else if (direction.X > 0) {
					Flipped = false;
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		return(velocity);
	}
	public Vector3 Fight(Vector3 velocity, double delta){
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;
		if ((Input.IsActionJustPressed("up") || Input.IsActionJustPressed("jump")) && IsOnFloor())
			velocity.Y = JumpVelocity;
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			if (direction.X < 0) {
				Flipped = true;
			}
			else if (direction.X > 0) {
					Flipped = false;
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		return(velocity);
	}
	public void Animate(){
		GetNode<AnimatedSprite3D>("sprite").Play("ready");
		GetNode<AnimatedSprite3D>("sprite").FlipH = Flipped;
	}
}

