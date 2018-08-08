using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : SemiSingleton<PlayerController>
{
    [Title ("Player Movement")]
    [SerializeField] float _moveSpeed = 1.0f;
    [SerializeField] float _decelerationRate = 1.0f;
    [SerializeField] float _movementDeadZone = 0.01f;
    [SerializeField] Rigidbody _playerRB;
    [SerializeField] Transform _cameraOffset;

    [Title ("Player Rotation")]
    [SerializeField] float _rotSpeed = 1.0f;
    [SerializeField] float _rotationDeadzone = 0.1f;
    [SerializeField] float _rotationOffset = -90.0f;

    [Title ("Player Attack")]
    [SerializeField] int _playerDamage;
    [SerializeField] Weapon _playerWeapon;
    [SerializeField] TimedEvent _attackEvent;

    bool _isAttacking;

    [Title ("Effects")]
    [SerializeField] Animator _animator;
    [SerializeField] ParticleSystem _slashPS;
    [SerializeField] List<AudioSource> _slashSound;

    [Title ("HealthBar")]
    [SerializeField] Health _health;
    [SerializeField] Image _healthBar;

    private void Awake ()
    {
        _playerWeapon._Damage = _playerDamage;
        _cameraOffset = GameObject.FindGameObjectWithTag ("CamOffset").transform;
    }

    void FixedUpdate ()
    {
        PlayerMovement ();
    }

    private void Update ()
    {
        if (Input.GetMouseButtonDown (0) && _attackEvent.IsEventReady ())
        {
            _attackEvent.TriggerEvent (this);
            _attackEvent.OnEventComplete (OnAttackComplete);
        }
    }

    private void PlayerMovement ()
    {
        if (_isAttacking)
        {
            StopMoving ();
            return;
        }

        Vector3 movement = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
        Vector3 finalVelocity = Vector3.zero;

        if (movement.magnitude > _movementDeadZone)
        {
            _animator.SetBool ("isRunning", true);
            Vector3 newMovement = _cameraOffset.forward * movement.normalized.z + _cameraOffset.right * movement.normalized.x;
            newMovement = Vector3.ClampMagnitude (newMovement, 1 / Mathf.Sqrt (2)) * Mathf.Sqrt (2);

            finalVelocity = newMovement * _moveSpeed * Time.fixedDeltaTime;
            _playerRB.velocity = finalVelocity;
        }
        else
        {
            StopMoving ();
        }

        if (finalVelocity.magnitude > _rotationDeadzone)
        {
            transform.rotation = Quaternion.LookRotation (finalVelocity, Vector3.up);
        }
    }

    private void StopMoving ()
    {
        _playerRB.velocity *= _decelerationRate * Time.fixedDeltaTime;
        _animator.SetBool ("isRunning", false);
    }

    private void RotateToMouse ()
    {
        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
        Vector3 mouseOnScreen = (Vector3) Camera.main.ScreenToViewportPoint (Input.mousePosition);

        float angle = AngleBetweenTwoPoints (positionOnScreen, mouseOnScreen);

        Quaternion _newQuat = Quaternion.Euler (new Vector3 (0f, -angle - _rotationOffset, 0f));
        transform.rotation = Quaternion.Lerp (transform.rotation, _newQuat, Time.time * _rotSpeed);
    }

    public void PlayerAttack ()
    {
        _isAttacking = true;
        RotateToMouse ();
        _playerWeapon.EnableWeapon ();
        _animator.SetBool ("isAttacking", true);
        _slashPS.Clear ();
        _slashPS.Play ();
        _slashSound.GetRandomFromList ().Play ();
    }

    void OnAttackComplete ()
    {
        _isAttacking = false;
        _playerWeapon.DisableWeapon ();
        _animator.SetBool ("isAttacking", false);
    }

    public void OnDamaged ()
    {
        _healthBar.fillAmount = _health._GetHealthPercent;
    }

    float AngleBetweenTwoPoints (Vector3 a, Vector3 b)
    {
        return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}