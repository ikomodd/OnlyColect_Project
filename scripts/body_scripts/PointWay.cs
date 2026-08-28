using Godot;
using System;

public partial class PointWay : Node2D {

	[Export]
	public string DestinyPath;

	[Export]
	public string PlayerTargetName = "";

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

	public void Enter() {

		var stateManager = GetNode<GameState>("/root/GameState");

		var playerCharacter = GetTree().CurrentScene.GetNode<PlayerCharacter>("PlayerCharacter");
		var playerTargetPosition = Position - playerCharacter.Position;

		stateManager.SetData("player_target_position", playerTargetPosition);
		stateManager.SetData("player_target_instance_name", PlayerTargetName);

		GetNode<UserInterface>("/root/UserInterface").ChangeCurtain(false, CurtainAction);
	}
}
