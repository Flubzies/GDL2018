using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class SettingsMenu : Menu<SettingsMenu>
	{
		public void OnClickRestart ()
		{
			Debug.Log ("Restart");
		}

		public void OnClickResume ()
		{
			Debug.Log ("Resume");
		}

		public void OnClickExit ()
		{
			Debug.Log ("Exit");
		}
	}
}