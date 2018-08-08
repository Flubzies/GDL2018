using UnityEngine;

/// <summary>
/// A sub version of the Singleton (No Don't Destroy on Load.)
/// </summary>
public class SemiSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	private static object _lock = new object ();

	public static T _Instance
	{
		get
		{
			lock (_lock)
			{
				if (_instance == null)
				{
					#region Try to find the instance.
					_instance = (T) FindObjectOfType (typeof (T));

					if (FindObjectsOfType (typeof (T)).Length > 1)
					{
						Debug.LogError ("[SemiSingleton] Something went really wrong " +
							" - there should never be more than 1 singleton!" +
							" Reopening the scene might fix it.");
						return _instance;
					}
					#endregion
					#region Make a new instance.
					if (_instance == null)
					{
						GameObject singleton = new GameObject ();
						_instance = singleton.AddComponent<T> ();
						singleton.name = "(singleton) " + typeof (T).ToString ();

						Debug.Log ("[SemiSingleton] An instance of " + typeof (T) +
							" is needed in the scene, so '" + singleton +
							"' .");
					}
					else
					{
						Debug.Log ("[SemiSingleton] Using instance already created: " +
							_instance.gameObject.name);
					}
					#endregion
				}
				return _instance;
			}
		}
	}
}