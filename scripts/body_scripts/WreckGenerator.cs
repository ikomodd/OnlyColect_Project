using Godot;
using System;

public partial class WreckGenerator : StaticBody2D {

	private ShaderMaterial conveyorShader = null;

	private PackedScene wreckFile = null;
	private Area2D conveyorArea = null;
	private Node2D wreckFolder = null;

	[Export]
	private float conveyorDirection = -1f;

	private const float CONVEYOR_SPEED = 10f;

	private const float GENERATOR_DELAY = 1.5f;
	private ulong prevGeneratorTick = 0;

	[Export]
	bool enabled = true;

	// ---------------------------------

	private void Generate() {

		if (wreckFolder.GetChildren().Count >= 10)
			return;

		var randomY = GD.RandRange(-16, 16);

		var wreckInstance = wreckFile.Instantiate<Wreck>();
		wreckInstance.Position = conveyorArea.GlobalPosition + new Vector2(0f, randomY);

		wreckFolder.AddChild(wreckInstance);
	}

	// ------------------------------------------

	public override void _Ready() {
		base._Ready();

		if (conveyorDirection == 1f) {

			GetNode<Sprite2D>("ConveyorSprite").RotationDegrees = 180f;
			GetNode<Sprite2D>("ConveyorSprite").Position += new Vector2(0, -16f);

		}

		conveyorShader = (ShaderMaterial)GetNode<Sprite2D>("ConveyorSprite").Material;

		conveyorArea = GetNode<Area2D>("ConveyorArea");
		wreckFolder = GetTree().CurrentScene.GetNode<Node2D>("Wrecks");
		wreckFile = GD.Load<PackedScene>("res://actors/wreck.tscn");
	}

	public override void _Process(double delta) {
		base._Process(delta);

		var currentTick = Time.GetTicksMsec();

		if (enabled) {
			if (currentTick - prevGeneratorTick > GENERATOR_DELAY * 1000) {

				Generate();
				prevGeneratorTick = currentTick;
			}

			var conveyorContent = conveyorArea.GetOverlappingBodies();

			foreach (Node2D node in conveyorContent) {

				if (node is CharacterBody2D charBody) {

					charBody.Velocity += new Vector2(CONVEYOR_SPEED * conveyorDirection, 0f);
				}
			}
		}

		conveyorShader.SetShaderParameter("u_enabled", enabled);
	}
}
