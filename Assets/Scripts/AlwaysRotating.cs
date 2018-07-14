using UnityEngine;

public class AlwaysRotating : MonoBehaviour
{
	[SerializeField] float _speed = 1.0f;

	private void Update ()
	{
		transform.Rotate (new Vector3 (0.0f, _speed * Time.deltaTime, 0f));
	}
}