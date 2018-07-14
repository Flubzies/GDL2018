using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform _target;
	public float _damping = 5f;
	Vector3 _offset;

	void Start ()
	{
		_offset = transform.position - _target.position;
	}

	void LateUpdate ()
	{
		Vector3 _targetCamPos = _target.position + _offset;
		transform.position = Vector3.Lerp (transform.position, _targetCamPos, _damping * Time.deltaTime);
	}
}