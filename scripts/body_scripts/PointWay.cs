using Godot;
using System;

public partial class PointWay : Node2D {

	[Export]
	public string DestinyPath;

	[Export]
	public string PlayerTargetName = "";

	//

	private void CurtainClosed() {

		var userInterface = GetNode<UserInterface>("/root/UserInterface");

		userInterface.OnCurtainClosed -= CurtainClosed;
		userInterface.OpenCurtain();

		var sceneDestiny = GD.Load<PackedScene>(DestinyPath);

		if (sceneDestiny != null)
			GetTree().ChangeSceneToPacked(sceneDestiny);
		else
			GD.PrintErr(" Erro durante tentativa de troca de cena por um Way: Destino inválido");
	}

	public void Enter() {

		var userInterface = GetNode<UserInterface>("/root/UserInterface");
		var stateManager = GetNode<GameState>("/root/GameState");

		var playerCharacter = GetTree().CurrentScene.GetNode<PlayerCharacter>("PlayerCharacter");
		var playerTargetPosition = Position - playerCharacter.Position;

		stateManager.SetData("player_target_instance_name", PlayerTargetName);

		userInterface.OnCurtainClosed += CurtainClosed;
		userInterface.CloseCurtain();
	}
}
