using UnityEngine;

namespace Managers
{
	[RequireComponent (typeof (CanvasGroup))]
	public class Menu<T> : Singleton<Menu<T>> where T : MonoBehaviour
	{
		[SerializeField] CanvasGroup _canvasGroup;
		[SerializeField] bool _disableCanvasGroupOnStart;
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
			if (_menuIsOpen) return;
			_canvasGroup.FadeOutCG (0.2f, true);
			_canvasGroup.blocksRaycasts = true;
			Time.timeScale = 0.0f;
			_menuIsOpen = true;
		}

		public void CloseMenu ()
		{
			if (!_menuIsOpen) return;
			_canvasGroup.FadeInCG (0.2f, true);
			_canvasGroup.blocksRaycasts = true;
			Time.timeScale = 1.0f;
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