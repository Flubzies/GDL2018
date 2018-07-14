using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I am reusing most of this script from GHAP 2017-2018.
public static class ExtensionMethods
{
	public static void RotateTowardsVector (this Transform trans_, Vector2 vel_, float rotSpeed_ = 4.0f, float angleOffset_ = 0.0f)
	{
		float angle = Mathf.Atan2 (vel_.y, vel_.x) * Mathf.Rad2Deg;
		trans_.rotation = Quaternion.Lerp (trans_.rotation, Quaternion.AngleAxis (angle + angleOffset_, Vector3.forward), Time.deltaTime * rotSpeed_);
	}

	public static Vector3 ToInt (this Vector3 vec_)
	{
		vec_.x.ToInt ();
		vec_.y.ToInt ();
		vec_.z.ToInt ();
		return vec_;
	}

	public static int ToInt (this float float_)
	{
		int i = Mathf.RoundToInt (float_);
		return i;
	}

	public static bool IsEven (this int int_)
	{
		if (int_ % 2 != 0) return false;
		return true;
	}

	public static T GetRandomFromList<T> (this List<T> list_)
	{
		return list_[UnityEngine.Random.Range (0, list_.Count)];
	}

	public static float Remap (this float value_, float fromA_, float toA_, float fromB_, float toB_)
	{
		return (value_ - fromA_) / (toA_ - fromA_) * (toB_ - fromB_) + fromB_;
	}

	public static Vector3 Remap (this Vector3 value_, float fromA_, float toA_, float fromB_, float toB_)
	{
		return value_ = new Vector3 ((value_.x - fromA_) / (toA_ - fromA_) * (toB_ - fromB_) + fromB_,
			(value_.y - fromA_) / (toA_ - fromA_) * (toB_ - fromB_) + fromB_,
			(value_.z - fromA_) / (toA_ - fromA_) * (toB_ - fromB_) + fromB_);
	}

	public static IEnumerator FadeInCG (this CanvasGroup cg_, bool unscaled_ = false, float fadeDuration_ = 0.2f)
	{
		Debug.Log ("Fadin in");

		float t = 0f;

		while (t < fadeDuration_)
		{
			if (unscaled_) t += Time.unscaledDeltaTime;
			else t += Time.deltaTime;

			cg_.alpha = t / fadeDuration_;
			yield return 0;
		}
		Debug.Log (t + "  " + fadeDuration_);
	}

	public static IEnumerator FadeOutCG (this CanvasGroup cg_, bool unscaled_ = false, float fadeDuration_ = 0.2f)
	{
		Debug.Log ("Fadin Out");
		while (fadeDuration_ > 0f)
		{
			if (unscaled_) fadeDuration_ -= Time.unscaledDeltaTime;
			else fadeDuration_ -= Time.deltaTime;

			cg_.alpha = fadeDuration_;
			yield return 0;
		}
	}

	private static System.Random rng = new System.Random ();

	public static void Shuffle<T> (this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next (n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}