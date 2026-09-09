using Godot;
using Godot.Collections;
using System;

public partial class Canteen : Area2D {

	private Node itemSlots = null;

	private byte prevUpdateDay = 1;

	private Godot.Collections.Dictionary canteenMenu = null;

	private Godot.Collections.Array dailyMenu = new Godot.Collections.Array();

	//

	private void ChangeMenu() {

		dailyMenu.Clear();

		foreach (var slot in itemSlots.GetChildren()) {

			var randomRarity = GD.RandRange(0, 100);
			Godot.Collections.Array rarityTarget = null;

			if (randomRarity < 10)
				rarityTarget = canteenMenu["legendary"].AsGodotArray(); // 10% de chance para legendary
			else if (randomRarity < 35)
				rarityTarget = canteenMenu["rare"].AsGodotArray(); // 25& de chance para rare
			else
				rarityTarget = canteenMenu["common"].AsGodotArray(); // 75% de chance para common

			var randomItemIndex = GD.RandRange(0, rarityTarget.Count - 1);
			var randomItem = rarityTarget[randomItemIndex].AsGodotDictionary().Duplicate();

			randomItem["buyed"] = false;

			dailyMenu.Add(randomItem);
		}
	}

	private void Buy(PlayerCharacter player_character) {

		var status = GetNode<PlayerStatus>("/root/PlayerStatus");

		byte slotNumber = 0;
		var minDistanceSquared = float.MaxValue;

		foreach (Node2D slot in itemSlots.GetChildren()) {

			var distanceSquared = (slot.GlobalPosition - player_character.GlobalPosition).LengthSquared();

			if (distanceSquared < minDistanceSquared) {

				slotNumber = ((byte)slot.GetMeta("slot_id"));
				minDistanceSquared = distanceSquared;
			}
		}

		var itemData = dailyMenu[slotNumber - 1].AsGodotDictionary();

		GD.Print(itemData);

		if (itemData["buyed"].AsBool()) {

			GD.Print("Esse item já foi comprado");
			return;
		}
		
		var itemPrice = itemData["price"].AsInt32();
		if (status.Coins < itemPrice) {

			GD.Print("Voce não tem dinheiro pra isso");
			return;
		}

		var satiety = itemData["satiety"].As<float>();

		status.Coins -= itemPrice;
		itemData["buyed"] = true;
		status.Hunger += satiety;
	}

	//

	public override void _Ready() {
		base._Ready();

		var gameTime = GetNode<GameTime>("/root/GameTime");
		var stateManager = GetNode<GameState>("/root/GameState");

		itemSlots = GetNode<Node>("Slots");

		var file = FileAccess.Open("res://resources/json_data/CanteenMenu.json", FileAccess.ModeFlags.Read);
		var source = file.GetAsText();

		canteenMenu = Json.ParseString(source).AsGodotDictionary();

		//

		if (stateManager.HasData("canteen_data")) {

			var canteenData = stateManager.GetData<Godot.Collections.Dictionary>("canteen_data");

			prevUpdateDay = canteenData["prev_update_day"].AsByte();

			GD.Print(prevUpdateDay);

			if (prevUpdateDay == gameTime.Day) {

				dailyMenu = canteenData["daily_menu"].AsGodotArray();
			}
			else
				ChangeMenu();
		}
		else
			ChangeMenu();
	}

	public override void _ExitTree() {
		base._ExitTree();

		var stateManager = GetNode<GameState>("/root/GameState");

		var canteenData = new Godot.Collections.Dictionary {

			{"prev_update_day", prevUpdateDay},
			{"daily_menu", dailyMenu}
		};

		stateManager.SetData("canteen_data", canteenData);
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (@event.IsActionPressed("interact")) {

			var playerCharacter = GetTree().CurrentScene.GetNode<PlayerCharacter>("PlayerCharacter");
			var colliders = GetOverlappingBodies();

			if (colliders.Contains(playerCharacter))
				Buy(playerCharacter);
		}
	}
}
