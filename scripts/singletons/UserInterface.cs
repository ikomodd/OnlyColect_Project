using Godot;
using System;
using System.Reflection.Metadata;

public partial class UserInterface : CanvasLayer {

	private Panel courtaine = null;

	public delegate void CurtainAction();

	//

	public void ChangeCurtain(bool state, CurtainAction action) {
		
		byte alpha = 255;

		if (state == true)
			alpha = 1;

		GD.Print("Rodando");

		var curtainTween = CreateTween();
		curtainTween.TweenProperty(courtaine, "modulate:a", alpha, 0.5f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.InOut);

		curtainTween.Finished += () => {

			if (!(action is null))
				action();
		};
	}

	//

	public override void _Ready() {
		base._Ready();

		courtaine = GetNode<Panel>("Curtain");
	}
}
