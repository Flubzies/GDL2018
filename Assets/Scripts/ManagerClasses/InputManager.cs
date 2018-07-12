using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	public static InputMode _InputMode { get; set; }

	private void Awake ()
	{
		_InputMode = InputMode.Game;
	}

}

public enum InputMode
{
	Undefined,
	Menu,
	Settings,
	Pause,
	Game
}