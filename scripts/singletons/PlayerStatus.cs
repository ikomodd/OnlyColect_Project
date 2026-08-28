using Godot;
using System;

public partial class PlayerStatus : Node {

	private ShaderMaterial healthBarShader = null;

	//

	private const float REGENERATION = 5.0f;
	private const float REGENERATION_DELAY = 5.0f;
	private ulong prevRegenerationTick = 0;

	private const float MAX_HEALTH = 100.0f;
	public float Health { get; private set; } = 100.0f;
	private float frontHealthBarScale = 1.0f;
	private float backHealthBarScale = 1.0f;
	private ulong damageTick = 0;

	public enum DamageType {

		UNDEFINED_DAMAGE = 0,
		FIRE_DAMAGE = 1,
		HIT_DAMAGE = 2
	}

	private DamageType prevDamageType = DamageType.UNDEFINED_DAMAGE;

	//

	public void TakeDamage(float damage, DamageType damage_type) {

		if (Health - damage <= 0) {

			Health = 0.0f;

			GD.Print("DIED");
			return;
		}
		else
			Health -= damage;

		prevDamageType = damage_type;
		damageTick = Time.GetTicksMsec();
	}

	//

	public override void _Ready() {
		base._Ready();

		var userInterface = GetNode<UserInterface>("/root/UserInterface");

		healthBarShader = (ShaderMaterial)userInterface.GetNode<Panel>("StatusContainer/HealthBar").Material;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		ulong currentTick = Time.GetTicksMsec();
		float healthPercent = Health / MAX_HEALTH;

		if (Mathf.Abs(frontHealthBarScale - healthPercent) > 0.001f) {

			frontHealthBarScale = Mathf.Lerp(frontHealthBarScale, healthPercent, 0.5f);
		}
		else if (Mathf.Abs(backHealthBarScale - healthPercent) > 0.001f && currentTick - damageTick > 500) {

			backHealthBarScale = Mathf.Lerp(backHealthBarScale, healthPercent, 0.25f);
		}

		if (Health < MAX_HEALTH && currentTick - damageTick > 1000 && currentTick - prevRegenerationTick > REGENERATION_DELAY * 1000) {

			Health += REGENERATION;
			prevRegenerationTick = currentTick;
			damageTick = currentTick;
		}

		healthBarShader.SetShaderParameter("u_damageType", (int)prevDamageType);
		healthBarShader.SetShaderParameter("u_frontBarStatus", frontHealthBarScale);
		healthBarShader.SetShaderParameter("u_backBarStatus", backHealthBarScale);
	}
}
