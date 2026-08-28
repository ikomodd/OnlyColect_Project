using Godot;
using System;

public partial class AreaWay : Area2D {

	[Export]
	public string DestinyPath;

	//

	private void CurtainAction() {

		GD.Print("action");

		GetNode<UserInterface>("/root/UserInterface").ChangeCurtain(true, null);

		var sceneDestiny = GD.Load<PackedScene>(DestinyPath);

		if (sceneDestiny != null)
			GetTree().ChangeSceneToPacked(sceneDestiny);
		else
			GD.PrintErr(" Erro durante tentativa de troca de cena por um Way: Destino inválido");
	}

	private void Enter(Node2D node) {

		var sceneDestiny = GD.Load<PackedScene>(DestinyPath);

		if (node is PlayerCharacter) {

			GetNode<UserInterface>("/root/UserInterface").ChangeCurtain(false, CurtainAction);
		}
	}

	public override void _Ready() {
		base._Ready();

		BodyEntered += Enter;
	}
}
