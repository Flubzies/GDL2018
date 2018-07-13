using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	public InputMode _InputMode { get; set; }

	public InputMode _initialInputMode = InputMode.MainMenu;

	protected override void Awake ()
	{
		base.Awake ();
		_InputMode = _initialInputMode;
	}

}

public enum InputMode
{
	Undefined,
	MainMenu,
	Settings,
	Pause,
	Game
}