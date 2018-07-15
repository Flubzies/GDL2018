using UnityEngine;

namespace Managers
{
	public class ApplicationManager : MonoBehaviour
	{
		public static ApplicationManager _instance = null;
		void Awake ()
		{
			if (_instance == null)
				_instance = this;
			else if (_instance != this) Destroy (gameObject);

			DontDestroyOnLoad (gameObject);
		}

		private void Update ()
		{
			if (Input.GetKeyDown (KeyCode.P))
			{
				SettingsMenu._instance.ToggleMenu ();
			}
		}
	}
}