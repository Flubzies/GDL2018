using UnityEngine;

namespace Managers
{
	public class ApplicationManager : Singleton<ApplicationManager>
	{
		[SerializeField] SettingsMenu _settingsMenu;

		private void Update ()
		{
			if (InputManager._instance._InputMode == InputMode.Game || InputManager._instance._InputMode == InputMode.Settings)
			{
				if (Input.GetKeyDown (KeyCode.P))
				{
					_settingsMenu.ToggleMenu ();
				}
			}
		}
	}
}