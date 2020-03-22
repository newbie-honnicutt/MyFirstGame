using Godot;
using System;

public class Mob : RigidBody2D
{
	[Export]
	public int MinSpeed = 150;
	
	[Export]
	public int MaxSpeed = 250;
	
	private String[] _mobTypes = {"fly", "swim", "walk"};
	
	static private Random _random = new Random();
	
	public override void _Ready()
	{
		GetNode<AnimatedSprite>("AnimatedSprite").Animation = _mobTypes[_random.Next(0, _mobTypes.Length)];
	}
	
	public void _on_Visibility_screen_exited()
	{
		QueueFree();
	}
	
	public void OnStartGame()
	{
		QueueFree();
	}
}
