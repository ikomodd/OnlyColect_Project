using System;
using Godot;
using static System.Net.Mime.MediaTypeNames;

public partial class PlayerStatus : Node {

	// Refs

	private RichTextLabel coinsLabel = null;
	private StatusBar hungerBar = null;
	private StatusBar healthBar = null;

	// Coins

	private const int DAILY_PAYMENT = 60;
	private int _coins = 0;

	public int Coins {

		get => _coins;
		set {

			if (value < 0) value = 0;

			_coins = value;
			coinsLabel.Text = "$" + value.ToString();
		}
	}

	// Fome

	private const float MAX_HUNGER = 100.0f;
	private const float HUNGER_COMSUMPTION = 2.0f;
	private const float HUNGER_DAMAGE = 5.0f;
	private const ulong HUNGER_DELAY = 5000;

	private float _hunger = MAX_HUNGER;
	public float Hunger {

		get => _hunger;
		set {

			_hunger = value;
			_hunger = Mathf.Clamp(_hunger, 0, MAX_HUNGER);

			if (_hunger == 0)
				Health -= HUNGER_DAMAGE;

			hungerBar.LossTick = Time.GetTicksMsec();
		}
	}

	// Regeneração da vida

	private const float REGENERATION = 5.0f;
	private const ulong REGENERATION_DELAY = 5000;

	// Vida

	private const float MAX_HEALTH = 100.0f;

	private float _health = MAX_HEALTH;
	public float Health {

		get => _health;
		set {

			_health = value;
			_health = Mathf.Clamp(_health, 0, MAX_HEALTH);

			if (_health <= 0)
				GD.Print("DIED");

			healthBar.LossTick = Time.GetTicksMsec();
		}
	}

	//

	//

	public override void _Ready() {
		base._Ready();

		var userInterface = GetNode<UserInterface>("/root/UserInterface");

		coinsLabel = userInterface.GetNode<RichTextLabel>("StatusContainer/CoinsLabel");
		hungerBar = userInterface.GetNode<StatusBar>("StatusContainer/HungerBar");
		healthBar = userInterface.GetNode<StatusBar>("StatusContainer/HealthBar");

		Coins = DAILY_PAYMENT;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		var currentTick = Time.GetTicksMsec();

		// Health

		if (Health < MAX_HEALTH && Hunger > MAX_HUNGER / 2.0f && currentTick - healthBar.LossTick > REGENERATION_DELAY) {

			Health += REGENERATION;
			Hunger -= 2.0f;

			healthBar.LossTick = currentTick;
		}

		hungerBar.Update(Hunger / MAX_HUNGER, currentTick);
		healthBar.Update(Health / MAX_HEALTH, currentTick);
	}
}
