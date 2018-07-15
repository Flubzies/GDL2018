using UnityEngine;

public static class SafeDestroy
{
	static T DestroyObject<T> (T obj_, float deathTimer_) where T : Object
	{
		if (!Application.isPlaying) Object.DestroyImmediate (obj_);
		else
		{
			Object.Destroy (obj_, deathTimer_);
			Debug.Log ("AA");
		}
		return null;
	}

	public static T DestroyGameObject<T> (T component_, float deathTimer_ = 0.0f) where T : Component
	{
		if (component_ != null) DestroyObject (component_.gameObject, deathTimer_);
		return null;
	}
}