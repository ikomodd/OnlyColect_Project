using Godot;
using System;

public partial class PassageWay : Node2D {

	[Export]
	PackedScene SceneDestiny = null;

	//

	public void Enter() {

		if (SceneDestiny != null)
			GetTree().ChangeSceneToPacked(SceneDestiny);
		else
			GD.PrintErr("Tentativa de troca de cena por um PassageWay mas parâmetro SceneDestiny é indefinido");
	}
}
