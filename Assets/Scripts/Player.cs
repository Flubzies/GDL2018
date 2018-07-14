using DG.Tweening;
using Managers;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.0f;
    [SerializeField] float _rotSpeed = 1.0f;

    Rigidbody _rb;

    void FixedUpdate ()
    {
        if (InputManager._instance._InputMode == InputMode.Game && Input.GetMouseButton (1)) _rb.velocity = _moveSpeed * transform.forward;
        else _rb.velocity = Vector3Int.zero;

        // Vector3 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
        // Vector3 mouseOnScreen = (Vector3) Camera.main.ScreenToViewportPoint (Input.mousePosition);

        Vector2 mousePos = Input.mousePosition;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));

        Vector3 newPos = Vector3.zero;

        newPos.y = Mathf.Atan2 ((screenPos.y - transform.position.y), (screenPos.x - transform.position.x)) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler (newPos);

        // float angle = AngleBetweenTwoPoints (positionOnScreen, mouseOnScreen);

        // Quaternion New = Quaternion.Euler (new Vector3 (0f, -angle, 0f));
        // transform.rotation = Quaternion.Lerp (transform.rotation, New, Time.time * _rotSpeed);
    }

    // float AngleBetweenTwoPoints (Vector3 a, Vector3 b)
    // {
    //     return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    // }

    private void OnValidate ()
    {
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody> ();
            Debug.Log ("Assigning RB: Status: " + _rb);
        }
    }
}