using Godot;
using System;

public partial class Wreck : CharacterBody2D {

	private const float DRAG = 0.9f;

	[Export]
	public float Weight = 1;

	[Export]
	public Texture Texture = null;

	// ---------------------------------------

	public void Eject(Vector2 drop_velocity) {

		Velocity = drop_velocity;
	}

	// -------------------------------------------------

	public override void _Ready() {
		base._Ready();

		// Open file
		
		var file = FileAccess.Open("res://resources/json_data/wrecks.json", FileAccess.ModeFlags.Read);
		var source = file.GetAsText();

		// Parse to GodotArray and get a random element

		var data = Json.ParseString(source).AsGodotArray();
		var randomIndex = GD.RandRange(0, data.Count - 1);
		var currentWreck = data[randomIndex].AsGodotDictionary();

		// Define the variables
		
		Vector2I sourcePosition = new Vector2I(currentWreck["source_pos"].AsGodotArray()[0].As<int>(), currentWreck["source_pos"].AsGodotArray()[1].As<int>());
		GetNode<Sprite2D>("Sprite").FrameCoords = sourcePosition;

		Weight = currentWreck["weight"].As<int>();
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		Velocity *= DRAG;

		MoveAndSlide();
	}
}
