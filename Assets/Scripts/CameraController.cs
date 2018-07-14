using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header ("Camera Movement")]
	[SerializeField] float _damping = 1.5f;
	[SerializeField] Transform _camOffset;

	[Header ("Camera Zoom")]
	[SerializeField] float _minOrthagraphicSize = 1.0f;
	[SerializeField] float _maxOrthagraphicSize = 10.0f;
	[Range (0, 1000)][SerializeField] float _mapZoomSpeed = 1.0f;

	[SerializeField] Camera _camera;

	float _tempFloat = 0.0f;

	void Update ()
	{
		if (InputManager._instance._InputMode == InputMode.Game)
		{
			CamMovementPC ();
		}
	}

	private void CamMovementPC ()
	{
		_tempFloat = Input.GetAxisRaw ("Horizontal");
		if (Mathf.Abs (_tempFloat) > 0.1)
		{
			//transform.position = Vector3.Lerp (transform.position, transform.position + _camOffset.right * _tempFloat, _damping * Time.deltaTime);
			transform.Translate (_camOffset.right * _tempFloat * _damping * Time.deltaTime);
		}
		_tempFloat = Input.GetAxisRaw ("Vertical");
		if (Mathf.Abs (_tempFloat) > 0.1)
		{
			//transform.position = Vector3.Lerp (transform.position, transform.position + _camOffset.f * _tempFloat, _damping * Time.deltaTime);
			transform.Translate (_camOffset.forward * _tempFloat * _damping * Time.deltaTime);
		}
	}

	float _finalCamScale;
	void ScrollZoom ()
	{
		_tempFloat = Input.GetAxis ("Mouse ScrollWheel");
		if (_tempFloat != 0f)
		{
			_finalCamScale = _camera.orthographicSize + _tempFloat;
			_camera.orthographicSize = Mathf.Lerp (_camera.orthographicSize, _finalCamScale * _tempFloat, _mapZoomSpeed * Time.deltaTime);
			Mathf.Clamp (_camera.orthographicSize, _minOrthagraphicSize, _maxOrthagraphicSize);
		}
	}
}