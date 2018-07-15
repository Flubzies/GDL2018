using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class SettingsMenu : Menu<SettingsMenu>
	{

		public void OnClickRestart ()
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		public void OnClickResume ()
		{
			CloseMenu ();
		}

		public void OnClickExit ()
		{
			CloseMenu ();
		}
	}
}