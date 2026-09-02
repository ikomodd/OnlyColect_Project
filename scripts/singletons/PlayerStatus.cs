using Godot;
using System;

public partial class PlayerStatus : Node {

	// Refs

	private StatusBar hungerBar = null;
	private StatusBar healthBar = null;

	// Coins

	public uint Coins { get; private set; } = 0;

	// Fome

	private const float MAX_HUNGER = 100.0f;
	private const float HUNGER_COMSUMPTION = 2.0f;
	private const float HUNGER_DAMAGE = 5.0f;
	private const ulong HUNGER_DELAY = 5000;

	public float Hunger { get; private set; } = MAX_HUNGER;

	// Regeneração da vida

	private const float REGENERATION = 5.0f;
	private const ulong REGENERATION_DELAY = 5000;

	// Vida

	private const float MAX_HEALTH = 100.0f;
	public float Health { get; private set; } = MAX_HEALTH;

	//

	public void Action() {

		Hunger -= HUNGER_COMSUMPTION;
		Hunger = Mathf.Clamp(Hunger, 0, MAX_HUNGER);

		if (Hunger <= 0)
			TakeDamage(HUNGER_DAMAGE);
	}

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

		if (Health < MAX_HEALTH && Hunger > MAX_HUNGER / 2.0f && currentTick - healthBar.LossTick > REGENERATION_DELAY) {

			Health += REGENERATION;
			healthBar.LossTick = currentTick;
		}

		// Hunger

		//if (currentTick - hungerBar.LossTick > HUNGER_DELAY) {

		//	if (Hunger > 0)
		//		Hunger -= HUNGER_COMSUMPTION;
		//	else
		//		TakeDamage(HUNGER_DAMAGE);

		//	hungerBar.LossTick = currentTick;
		//}

		hungerBar.Update(Hunger / MAX_HUNGER, currentTick);
		healthBar.Update(Health / MAX_HEALTH, currentTick);
	}
}
