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

		protected override void Awake ()
		{
			if (_disableCanvasGroupOnStart)
			{
				_canvasGroup.alpha = 0;
				_canvasGroup.blocksRaycasts = false;
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
			_canvasGroup.alpha = 1.0f;
			_canvasGroup.blocksRaycasts = true;
			Time.timeScale = 0.0f;
			_menuIsOpen = true;
		}

		public void CloseMenu ()
		{
			Debug.Log ("Closing");
			if (!_menuIsOpen) return;
			_canvasGroup.alpha = 0.0f;
			_canvasGroup.blocksRaycasts = true;
			Time.timeScale = 1.0f;
			_menuIsOpen = false;
		}

		IEnumerator FadeInCG ()
		{
			float t = 0f;

			while (t < _menuFadeDuration)
			{
				t += Time.unscaledDeltaTime;
				_canvasGroup.alpha = t / _menuFadeDuration;
				yield return 0;
			}
		}

		IEnumerator FadeOutCG ()
		{
			Debug.Log ("Fadin Out");
			while (_menuFadeDuration > 0f)
			{
				_menuFadeDuration -= Time.unscaledDeltaTime;
				_canvasGroup.alpha = _menuFadeDuration;
				yield return 0;
			}
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