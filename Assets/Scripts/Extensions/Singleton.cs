using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T _instance = null;
	[SerializeField] protected bool _dontDestroyOnLoad = false;

	protected virtual void Awake ()
	{
		if (_instance == null) _instance = this as T;
		else if (_instance != this)
		{
			Debug.LogError ("Multiple objects of " + typeof (T) + " found in scene!");
			Destroy (gameObject);
		}
		if (_dontDestroyOnLoad) DontDestroyOnLoad (gameObject);
	}
}