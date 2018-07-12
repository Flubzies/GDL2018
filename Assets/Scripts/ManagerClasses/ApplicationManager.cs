using UnityEngine;

namespace Managers
{
	public class ApplicationManager : Singleton<ApplicationManager>
	{
		[SerializeField] SettingsMenu _settingsMenu;

		private void Update ()
		{
			Debug.Log (InputManager._instance);
			if (InputManager._instance._InputMode == InputMode.Game)
			{
				if (Input.GetKeyDown (KeyCode.P))
				{
					_settingsMenu.OpenMenu ();
				}
			}
		}
	}
}