using Godot;
using System;

public partial class Furnace : StaticBody2D {

	private ShaderMaterial fuelBarShaderMaterial = null;
	private ShaderMaterial particleShaderMaterial = null;

	private GpuParticles2D particleEmiter = null;
	private Sprite2D mounthSprite = null;
	private Panel fuelBar = null;
	private Area2D mounthArea = null;
	private Area2D damageArea = null;

	// ---------------------------------------

	private const float FUEL_CONSUMPTION = 0.5f;
	private const float FUEL_MAX = 100f;

	private float fuelCount = 50f;

	private const float BURN_DELAY = 1f;
	private ulong prevBurnTick = 0;

	private bool mounthOpened = false;

	// ------------------------------------

	private void UpdateBar() {

		var fuelCountPercent = fuelCount / FUEL_MAX;

		fuelBarShaderMaterial.SetShaderParameter("u_progress", fuelCountPercent);
	}

	private void UpdateMounth() {

		var fuelCountPercent = fuelCount / FUEL_MAX;

		// Close

		if (mounthOpened) {

			// Disable particles and close the mounth

			if (fuelCountPercent == 1.0f) {

				particleEmiter.Emitting = false;
				mounthOpened = false;

				var mounthTween = CreateTween();
				mounthTween.TweenProperty(mounthSprite, "position", new Vector2(0f, 0f), 0.5f).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.In);
			}

			// give damage if has a player in damageArea

			var damageAreaContent = damageArea.GetOverlappingBodies();

			foreach (var body in damageAreaContent) {

				if (body is PlayerCharacter) {

					var playerStatus = GetNode<PlayerStatus>("/root/PlayerStatus");
					playerStatus.TakeDamage(1.0f, PlayerStatus.DamageType.FIRE_DAMAGE);
				}
			}
		}

		// Open

		// Enable particles and open the mounth

		else if (!mounthOpened && fuelCountPercent < 0.5f) {

			particleEmiter.Emitting = true;
			mounthOpened = true;

			var mounthTween = CreateTween();
			mounthTween.TweenProperty(mounthSprite, "position", new Vector2(0f, -16f), 2.0f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
		}

		particleShaderMaterial.SetShaderParameter("u_size", fuelCountPercent);
	}

	private void MounthEntered(Node2D node) {

		if (node is Wreck wreckInstance) {

			if (mounthOpened) {

				fuelCount += wreckInstance.Weight;
				fuelCount = Math.Clamp(fuelCount, 0f, 100f);

				wreckInstance.QueueFree();
			}
			else {

				wreckInstance.Velocity = new Vector2(0f, 200f);
			}
		}
	}

	// ----------------------------

	public override void _Ready() {
		base._Ready();

		particleEmiter = GetNode<GpuParticles2D>("ParticleEmiter");
		particleEmiter.Emitting = true;

		fuelBar = GetNode<Panel>("fuelBar");

		fuelBarShaderMaterial = (ShaderMaterial)fuelBar.Material;
		particleShaderMaterial = (ShaderMaterial)particleEmiter.Material;

		mounthSprite = GetNode<Sprite2D>("MounthSprite");

		mounthArea = GetNode<Area2D>("MounthArea");
		mounthArea.BodyEntered += MounthEntered;

		damageArea = GetNode<Area2D>("DamageArea");
	}

	public override void _Process(double delta) {
		base._Process(delta);

		var currentTick = Time.GetTicksMsec();

		if (currentTick - prevBurnTick > BURN_DELAY * 1000) {

			fuelCount -= FUEL_CONSUMPTION;
			prevBurnTick = currentTick;
		}

		UpdateBar();
		UpdateMounth();
	}
}
