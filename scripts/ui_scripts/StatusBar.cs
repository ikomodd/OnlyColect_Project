using Godot;
using System;

public partial class StatusBar : Panel {

	private ShaderMaterial shaderMaterial = null;

	private float frontBarScale = 1.0f;
	private float backBarScale = 1.0f;

	public ulong LossTick = 0;

	//

	public void Update(float value_percent, ulong current_tick) {

		// Se frontBarScale != value_percent: da lerp de frontBarScale até value_percent (Não existe sinal de != ou == entre floats válido)

		if (Mathf.Abs(frontBarScale - value_percent) > 0.001f)

			frontBarScale = Mathf.Lerp(frontBarScale, value_percent, 0.5f);

		// Se backBarScale != value_percent, espera 500ms e da lerp de backBar até value_percent

		else if (Mathf.Abs(backBarScale - value_percent) > 0.001f && current_tick - LossTick > 500)

			backBarScale = Mathf.Lerp(backBarScale, value_percent, 0.25f);

		shaderMaterial.SetShaderParameter("u_frontBarStatus", frontBarScale);
		shaderMaterial.SetShaderParameter("u_backBarStatus", backBarScale);
	}

	//

	public override void _Ready() {
		base._Ready();

		shaderMaterial = (ShaderMaterial)Material;
	}
}
