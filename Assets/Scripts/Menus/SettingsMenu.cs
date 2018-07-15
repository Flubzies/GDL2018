using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class SettingsMenu : MonoBehaviour
	{
		public static SettingsMenu _instance = null;
		void Awake ()
		{
			if (_instance == null)
				_instance = this;
			else if (_instance != this) Destroy (gameObject);
		}

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