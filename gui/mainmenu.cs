using Godot;
using System;

public partial class mainmenu : Control
{
	void SoloPress()
	{
		loader gameLoader = GetNode<loader>("/root/GameLoader");
		gameLoader.LoadTo("res://maps/ae_canyon.tscn");
	}
}



