using Godot;
using System;

public partial class Canteen : Area2D {

	void Interact() {

		var playerStatus = GetNode<PlayerStatus>("/root/PlayerStatus");

		playerStatus.Eat(10.0f);
	}

	//

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (@event.IsActionPressed("interact")) {

			var playerCharacter = GetTree().CurrentScene.GetNode<PlayerCharacter>("PlayerCharacter");
			var colliders = GetOverlappingBodies();

			if (colliders.Contains(playerCharacter))
				Interact();
		}
	}
}
