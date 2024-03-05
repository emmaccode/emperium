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
	
}

