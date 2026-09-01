using Godot;
using System;

public partial class PlayerStatus : Node {

	// Refs

	private StatusBar hungerBar = null;
	private StatusBar energyBar = null;
	private StatusBar healthBar = null;

	// Coins

	public uint Coins { get; private set; } = 0;

	//  Energia

	private const float MAX_ENERGY = 100.0f;

	public float Energy { get; private set; } = 100.0f;

	// Fome

	private const float MAX_HUNGER = 100.0f;
	private const float HUNGER_COMSUMPTION = 2.0f;
	private const float HUNGER_DAMAGE = 5.0f;
	private const ulong HUNGER_DELAY = 5000;

	private ulong prevHungerTick = 0;

	public float Hunger { get; private set; } = 100.0f;

	// Regeneração da vida

	private const float REGENERATION = 5.0f;
	private const ulong REGENERATION_DELAY = 5000;
	private ulong prevRegenerationTick = 0;

	// Vida

	private const float MAX_HEALTH = 100.0f;
	public float Health { get; private set; } = 100.0f;

	//

	public void Eat(float food_count) {

		Hunger += food_count;
		Hunger = Mathf.Clamp(Hunger, 0, MAX_HUNGER);

		hungerBar.LossTick = Time.GetTicksMsec();
	}

	public void TakeDamage(float damage) {

		Health -= damage;
		Health = Mathf.Clamp(Health, 0, MAX_HEALTH);

		if (Health <= 0) {

			GD.Print("DIED");
		}

		healthBar.LossTick = Time.GetTicksMsec();
	}

	//

	public override void _Ready() {
		base._Ready();

		var userInterface = GetNode<UserInterface>("/root/UserInterface");

		hungerBar = userInterface.GetNode<StatusBar>("StatusContainer/HungerBar");
		healthBar = userInterface.GetNode<StatusBar>("StatusContainer/HealthBar");
	}

	public override void _Process(double delta) {
		base._Process(delta);

		var currentTick = Time.GetTicksMsec();

		// Health

		if (Health < MAX_HEALTH && Hunger > MAX_HUNGER / 2.0f && currentTick - prevRegenerationTick > REGENERATION_DELAY) {

			Health += REGENERATION;
			prevRegenerationTick = currentTick;
			healthBar.LossTick = currentTick;
		}

		// Hunger

		if (currentTick - prevHungerTick > HUNGER_DELAY) {

			if (Hunger > 0)
				Hunger -= HUNGER_COMSUMPTION;
			else
				TakeDamage(HUNGER_DAMAGE);

			prevHungerTick = currentTick;
			hungerBar.LossTick = currentTick;
		}

		hungerBar.Update(Hunger / MAX_HUNGER, currentTick);
		healthBar.Update(Health / MAX_HEALTH, currentTick);
	}
}
