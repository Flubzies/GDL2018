using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class OpeningMessage : MonoBehaviour
	{
		[SerializeField] CanvasGroup _cg;

		private void Awake ()
		{
			StartCoroutine (Message ());
		}

		IEnumerator Message ()
		{
			yield return new WaitForSeconds (10.0f);
			StartCoroutine (FadeIn ());
			yield return new WaitForSeconds (5.0f);
			StartCoroutine (FadeOut ());
		}

		IEnumerator FadeIn ()
		{
			float t = 0f;

			while (t < 1.0f)
			{
				t += Time.deltaTime;
				_cg.alpha = t / 1.0f;
				yield return 0;
			}

			_cg.blocksRaycasts = true;
		}

		IEnumerator FadeOut ()
		{
			float t = 1.0f;

			while (t > 0)
			{
				t -= Time.deltaTime;
				_cg.alpha = t;
				yield return 0;
			}
		}

	}
}