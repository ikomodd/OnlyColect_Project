using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D {

	private Sprite2D characterSprite = null;
	private AnimationPlayer animator = null;
	private Node2D wreckFolder = null;
	private Node2D interactableFolder = null;

	//

	private const float SPEED = 7000.0f;
	private const float DRAG = 0.7f;

	private const float INTERACT_RANGE = 64.0f;
	private const float GET_WRECK_RANGE = 64.0f;

	private const float DROP_FORCE = 600.0f;

	//

	private Vector2 prevDirection = new Vector2(0f, 1f);
	private Vector2 handlePosition = Vector2.Zero;
	private float animationDirection = 1.0f;

	public Wreck OnHands { get; private set; } = null;

	// -------------------------------------

	private void Interact() {

		if (interactableFolder == null)
			return;

		float minRangeSquared = INTERACT_RANGE * INTERACT_RANGE;

		Node2D nearestInteractable = null;
		float minDistanceSquared = float.MaxValue;

		foreach (Node2D node in interactableFolder.GetChildren()) {

			float distanceSquared = (Position - node.Position).LengthSquared();

			if (distanceSquared < minRangeSquared && distanceSquared < minDistanceSquared) {

				minDistanceSquared = distanceSquared;
				nearestInteractable = node;
			}
		}

		if (nearestInteractable is PointWay passageWay)
			passageWay.Enter();
	}

	private void GetWreck() {

		// If has a object in hands, drop her

		if (OnHands != null) {
			Drop();
			return;
		}

		if (wreckFolder == null)
			return;

		// Collect data

		float interactRangeSquared = GET_WRECK_RANGE * GET_WRECK_RANGE;

		float minDistanceSquared = float.MaxValue;
		Node2D nearestWreck = null;

		// Get the nearest Node2D Wreck

		foreach (Node2D wreck in wreckFolder.GetChildren()) {

			float wreckDistanceSquared = (wreck.Position - Position).LengthSquared();

			if (wreckDistanceSquared < interactRangeSquared && wreckDistanceSquared < minDistanceSquared) {

				minDistanceSquared = wreckDistanceSquared;
				nearestWreck = wreck;
			}
		}

		if (nearestWreck == null)
			return;

		Wreck wreckInstance = nearestWreck as Wreck;

		OnHands = wreckInstance;
		wreckInstance.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
	}

	private void Drop() {

		OnHands.Position = Position + prevDirection * 16f;
		OnHands.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;

		Vector2 dropDirecrtion = Velocity + prevDirection * DROP_FORCE;
		OnHands.Eject(dropDirecrtion);

		OnHands = null;
	}

	private static Vector2 GetDirections() {

		Vector2 result = Vector2.Zero;

		if (Input.IsActionPressed("walk_up"))
			result.Y = -1f;

		else if (Input.IsActionPressed("walk_down"))
			result.Y = 1f;

		if (Input.IsActionPressed("walk_left"))
			result.X = 1f;
		else if (Input.IsActionPressed("walk_right"))
			result.X = -1f;

		return result;
	}

	// -------------------------------------------------

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (@event.IsActionPressed("get_wreck"))
			GetWreck();
		else if (@event.IsActionPressed("interact"))
			Interact();
	}

	public override void _Ready() {
		base._Ready();

		var stateManager = GetNode<GameState>("/root/GameState");

		if (stateManager.HasData("player_target_instance_name")) {

			var targetName = stateManager.GetData<string>("player_target_instance_name");
			var targetInstance = GetTree().CurrentScene.GetNode<Node2D>("Interactables/" + targetName);

			Position = targetInstance.Position;
		}

		//

		characterSprite = GetNode<Sprite2D>("CharacterSprite");
		animator = GetNode<AnimationPlayer>("Animator");

		handlePosition = GetNode<Node2D>("Handle").Position;

		var currentScene = GetTree().CurrentScene;

		if (currentScene.HasNode("Wrecks"))
			wreckFolder = currentScene.GetNode<Node2D>("Wrecks");

		if (currentScene.HasNode("Interactables"))
			interactableFolder = currentScene.GetNode<Node2D>("Interactables");
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		var direction = GetDirections();

		if (direction.X != 0) animationDirection = direction.X;
		else if (direction.Y != 0) animationDirection = direction.Y;

		direction = direction.Normalized();

		if (direction != Vector2.Zero)
			prevDirection = direction;

		Vector2 acelleration = (direction * SPEED * (float)delta);

		Velocity += acelleration;
		Velocity *= DRAG;

		MoveAndSlide();

		//

		if (OnHands != null) {
			OnHands.Position = Position + handlePosition;
		}

		//

		string currentAnimation = "";

		characterSprite.Scale = new Vector2(animationDirection, 1.0f);

		if (direction != Vector2.Zero) {

			if (OnHands == null)
				currentAnimation = "walk";
			else
				currentAnimation = "walk_holding";
		}
		else {
			if (OnHands == null)
				currentAnimation = "idle";
			else
				currentAnimation = "idle_holding";
		}

		if (currentAnimation != animator.CurrentAnimation)
			animator.Play(currentAnimation);
	}
}
