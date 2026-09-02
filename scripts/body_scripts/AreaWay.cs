using System;
using Godot;
using static UserInterface;

public partial class AreaWay : Area2D {

	[Export]
	public string DestinyPath;

	//

	private void Enter(Node2D node) {

		var sceneDestiny = GD.Load<PackedScene>(DestinyPath);

		

		if (node is PlayerCharacter) {

			var userInterface = GetNode<UserInterface>("/root/UserInterface");

			userInterface.CloseCurtain();
			userInterface.OnCurtainClosed += () => {

				userInterface.OpenCurtain();

				var sceneDestiny = GD.Load<PackedScene>(DestinyPath);

				if (sceneDestiny != null)
					GetTree().ChangeSceneToPacked(sceneDestiny);
				else
					GD.PrintErr(" Erro durante tentativa de troca de cena por um Way: Destino inválido");
			};
		}
	}

	public override void _Ready() {
		base._Ready();

		BodyEntered += Enter;
	}
}
