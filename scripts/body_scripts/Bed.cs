using Godot;
using System;

public partial class Bed : Area2D {

	void Sleep() {

		var gameTime = GetNode<GameTime>("/root/GameTime");

		gameTime.Sleep();
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (@event.IsActionPressed("interact")) {

			var playerCharacter = GetTree().CurrentScene.GetNode<PlayerCharacter>("PlayerCharacter");
			var colliders = GetOverlappingBodies();

			if (colliders.Contains(playerCharacter))
				Sleep();
		}
	}
}
