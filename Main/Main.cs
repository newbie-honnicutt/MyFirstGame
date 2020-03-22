using Godot;
using System;

public class Main : Node
{
	[Export]
	public PackedScene Mob;
	
	private int _score;
	
	private Random _random = new Random();
	
	public override void _Ready()
	{
	}
	
	private float RandRange(float min, float max)
	{
		return (float)_random.NextDouble() * (max - min) + min;
	}
	
	private void game_over()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	public void new_game()
	{
		_score = 0;
		
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Position2D>("StartPosition");
		player.Start(startPosition.Position);
		
		GetNode<Timer>("StartTimer").Start();
		
		GetNode<AudioStreamPlayer>("Music").Play();
		
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
	}
	
	public void _on_StartTimer_timeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	
	public void _on_ScoreTimer_timeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}
	
	public void _on_MobTimer_timeout()
	{
		// Choose a random location on Path2D.
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.SetOffset(_random.Next());
		
		// Create a Mob instance and add it to the scene.
		var mobInstance = (RigidBody2D)Mob.Instance();
		AddChild(mobInstance);
		
		// Set the mob's direction perpendicular to the path direction.
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2; // pi/2 rad = 90 degrees
		
		// Set the mob's position to random location.
		mobInstance.Position = mobSpawnLocation.Position;
		
		// Set randomness to the direction.
		direction += RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mobInstance.Rotation = direction;
		
		// Choose the velocity.
		mobInstance.SetLinearVelocity(new Vector2(RandRange(150f, 250f), 0).Rotated(direction));
		
		GetNode("HUD").Connect("StartGame", mobInstance, "OnStartGame");
	}
}
