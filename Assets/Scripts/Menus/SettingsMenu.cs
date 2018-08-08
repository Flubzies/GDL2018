using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
	public class SettingsMenu : Menu<SettingsMenu>
	{
		[Header ("Buttons")]
		[SerializeField] Button Resume;
		[SerializeField] Button Restart;
		[SerializeField] Button Close;
		[SerializeField] Button Exit;

		private void Awake ()
		{
			CheckButtonState ();
		}

		public void OnClickRestart ()
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			CloseMenu ();
		}

		public void OnClickResume ()
		{
			CloseMenu ();
		}

		public void OnClickExit ()
		{
			Application.Quit ();
		}

		public void CheckButtonState (bool _isGameOver = false)
		{
			if (!_isGameOver)
			{
				Resume.interactable = true;
				Restart.interactable = true;
				Close.interactable = true;
				Exit.interactable = true;
				return;
			}
			else
			{
				Resume.interactable = false;
				Restart.interactable = true;
				Close.interactable = false;
				Exit.interactable = true;
				return;
			}
		}
	}
}