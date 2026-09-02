using Godot;
using System;
using System.Reflection.Metadata;

public partial class UserInterface : CanvasLayer {

	private Panel courtaine = null;

	public delegate void CurtainAction();
	public event CurtainAction OnCurtainClosed;

	//

	public void CloseCurtain() {
		
		byte alpha = 255;

		var curtainTween = CreateTween();
		curtainTween.TweenProperty(courtaine, "modulate:a", alpha, 0.5f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.InOut);

		curtainTween.Finished += () => {

			OnCurtainClosed?.Invoke();
		};
	}

	public void OpenCurtain() {

		byte alpha = 0;

		var curtainTween = CreateTween();
		curtainTween.TweenProperty(courtaine, "modulate:a", alpha, 0.5f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.InOut);
	}

	//

	public override void _Ready() {
		base._Ready();

		courtaine = GetNode<Panel>("Curtain");
	}
}
