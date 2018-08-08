using UnityEngine;

namespace Managers
{
	public class ApplicationManager : Singleton<ApplicationManager>
	{
		private void Update ()
		{
			if (Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown (KeyCode.Escape))
			{
				SettingsMenu._Instance.ToggleMenu ();
			}
		}
	}
}