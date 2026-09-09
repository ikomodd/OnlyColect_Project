using Godot;
using System;

public partial class GameTime : Node {

	private RichTextLabel clockLabel = null;

	//

	private const byte MAX_SLEEP_TIME = 3;
	private const byte SLEEP_TIME = 0;

	private const ulong MINUTE_TICKS = 1000;
	private ulong prevSecondTick = 0; 

	public byte Hours   { get; private set; } = 6;
	public byte Minutes { get; private set; } = 30;
	public byte Day { get; private set; } = 1;

	//

	public void Sleep() {

		var userInterface = GetNode<UserInterface>("/root/UserInterface");
		var status = GetNode<PlayerStatus>("/root/PlayerStatus");

		userInterface.OnCurtainClosed = () => {

			if (Hours > 6)
				status.Hunger = 1000;

			Hours = 6;
			Minutes = 30;
			Day++;

			userInterface.OnCurtainClosed = null;
			userInterface.OpenCurtain();
		};

		userInterface.CloseCurtain();
	}

	private void UpdateClock(ulong current_tick) {

		if (current_tick - prevSecondTick > MINUTE_TICKS) {

			if (Minutes < 59) Minutes++;
			else {

				Minutes = 0;

				if (Hours < 23) Hours++;
				else {

					Hours = 0;

					Day++;
				}
			}

			var hoursDisplay = (Hours < 10) ? "0" + Hours.ToString() : Hours.ToString();
			var minutesDisplay = (Minutes < 10) ? "0" + Minutes.ToString() : Minutes.ToString();
			var dayDisplay = (Day < 10) ? "0" + Day.ToString() : Day.ToString();

			clockLabel.Text = $"{hoursDisplay} : {minutesDisplay} {dayDisplay}";

			prevSecondTick = current_tick;
		}
	}

	//

	public override void _Ready() {
		base._Ready();

		var userInterface = GetNode<UserInterface>("/root/UserInterface");

		clockLabel = userInterface.GetNode<RichTextLabel>("ClockContainer/ClockLabel");
	}

	public override void _Process(double delta) {
		base._Process(delta);

		var currentTick = Time.GetTicksMsec();

		UpdateClock(currentTick);

		if (Hours >= 3 && Hours < 6)
			Sleep();
	}
}
