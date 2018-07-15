using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Title ("Player Movement")]
    [SerializeField] float _moveSpeed = 1.0f;
    [SerializeField] float _decelerationRate = 1.0f;
    [SerializeField] float _deadZone = 0.01f;
    [SerializeField] Transform _cameraOffset;

    [Title ("Player Rotation")]
    [SerializeField] float _rotSpeed = 1.0f;
    [SerializeField] float _rotationOffset = -90.0f;

    [Title ("Player Shooting")]
    [SerializeField] float _fireRange;
    [SerializeField] Transform _firePoint;

    [SerializeField] Rigidbody _rb;
    Vector3 _movement;
    Vector3 _shootDir;

    //Private Vars
    private Vector3 _mousePos;
    private Vector3 _direction;

    void FixedUpdate ()
    {
        PlayerMovement ();
        PlayerRotation ();
        PlayerShoot ();
    }

    private void PlayerMovement ()
    {
        _movement = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
        if (_movement.magnitude > _deadZone)
        {
            Vector3 newMovement = _cameraOffset.forward * _movement.normalized.z + _cameraOffset.right * _movement.normalized.x;
            newMovement = Vector3.ClampMagnitude (newMovement, 1 / Mathf.Sqrt (2)) * Mathf.Sqrt (2);

            _rb.velocity = newMovement * _moveSpeed * Time.fixedDeltaTime;
        }
        else _rb.velocity *= _decelerationRate * Time.fixedDeltaTime;
    }

    private void PlayerRotation ()
    {
        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
        Vector3 mouseOnScreen = (Vector3) Camera.main.ScreenToViewportPoint (Input.mousePosition);

        float angle = AngleBetweenTwoPoints (positionOnScreen, mouseOnScreen);

        Quaternion _newQuat = Quaternion.Euler (new Vector3 (0f, -angle - _rotationOffset, 0f));
        transform.rotation = Quaternion.Lerp (transform.rotation, _newQuat, Time.time * _rotSpeed);
    }

    private void PlayerShoot ()
    {
        if (Input.GetMouseButtonDown (0))
        {
            // RaycastHit hit;
            // Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            // if (Physics.Raycast (ray, out hit, 1000f, _shootLayer))
            // {
            //     if (hit.collider)
            //     {
            //         // Rotate to that position
            //         _shootDir = hit.point - transform.position;
            //         _shootDir.y = 0;
            //         Debug.DrawRay (_firePoint.forward + transform.position, _shootDir.normalized * _fireRange, Color.red, 2.0f);
            //     }
            // }

            // Projectile p = Instantiate (_projectile, _firePoint.forward + transform.position, Quaternion.identity, transform);
            // p.SetVelocity (_firePoint.forward);
            // SafeDestroy.DestroyGameObject (p, _bulletLifeTime);

        }
    }

    float AngleBetweenTwoPoints (Vector3 a, Vector3 b)
    {
        return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}