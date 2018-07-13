using System.Collections;
using UnityEngine;

namespace Managers
{
	[RequireComponent (typeof (CanvasGroup))]
	public class Menu<T> : Singleton<Menu<T>> where T : MonoBehaviour
	{
		[SerializeField] CanvasGroup _canvasGroup;
		[SerializeField] bool _disableCanvasGroupOnStart;
		[SerializeField] float _menuFadeDuration = 0.2f;
		bool _menuIsOpen;
		InputMode _previousInputMode;

		protected override void Awake ()
		{
			base.Awake ();
			_previousInputMode = InputManager._instance._initialInputMode;

			if (_disableCanvasGroupOnStart)
			{
				_menuIsOpen = true;
				CloseMenu ();
			}
		}

		public void ToggleMenu ()
		{
			if (_menuIsOpen) CloseMenu ();
			else OpenMenu ();
		}

		public void OpenMenu ()
		{
			Debug.Log ("Opening");
			if (_menuIsOpen) return;
			StartCoroutine (FadeIn ());
			Time.timeScale = 0.0f;
		}

		public void CloseMenu ()
		{
			Debug.Log ("Closing");
			if (!_menuIsOpen) return;
			StartCoroutine (FadeOut ());
			Time.timeScale = 1.0f;
		}

		IEnumerator FadeIn ()
		{
			float t = 0f;

			while (t < _menuFadeDuration)
			{
				t += Time.unscaledDeltaTime;
				_canvasGroup.alpha = t / _menuFadeDuration;
				yield return 0;
			}

			_previousInputMode = InputManager._instance._InputMode;
			InputManager._instance._InputMode = InputMode.Settings;
			_canvasGroup.blocksRaycasts = true;
			_menuIsOpen = true;
		}

		IEnumerator FadeOut ()
		{
			float t = _menuFadeDuration;

			while (t > 0)
			{
				t -= Time.unscaledDeltaTime;
				_canvasGroup.alpha = t;
				Debug.Log (_canvasGroup.alpha);
				yield return 0;
			}

			InputManager._instance._InputMode = _previousInputMode;
			_canvasGroup.blocksRaycasts = false;
			_menuIsOpen = false;
		}

		private void OnValidate ()
		{
			if (_canvasGroup == null)
			{
				_canvasGroup = GetComponent<CanvasGroup> ();
			}
		}

	}
}