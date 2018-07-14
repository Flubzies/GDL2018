using System.Collections;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
	[SerializeField] GameObject GameCamera;

	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.position = GameCamera.transform.position;
		gameObject.transform.rotation = GameCamera.transform.rotation;
		transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
	}
}