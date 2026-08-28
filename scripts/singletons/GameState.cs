using Godot;
using Godot.Collections;
using System;

public partial class GameState : Node {

	private Dictionary<string, Variant> state = new Dictionary<string, Variant>();

	//

	public bool HasData(string name) {

		return state.ContainsKey(name);
	}

	public void SetData(string name, Variant data) {

		state[name] = data;
	}

	public T GetData<[MustBeVariant] T>(string name) {

		return state[name].As<T>();
	}
}
