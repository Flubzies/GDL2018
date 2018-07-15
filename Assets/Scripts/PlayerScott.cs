using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerScott : MonoBehaviour
{
    [Title ("Player Movement")]
    [SerializeField] float _moveSpeed = 1.0f;
    [SerializeField] float _decelerationRate = 1.0f;
    [SerializeField] float _deadZone = 0.01f;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Transform _cameraOffset;

    [Title ("Player Rotation")]
    [SerializeField] float _rotationDeadZone = 0.1f;
    [SerializeField] float _rotSpeed = 1.0f;
    [SerializeField] float _rotationOffset = -90.0f;

    [Title ("Player Atttacking")]
    [SerializeField] Transform _firePoint;
    [SerializeField] float _attackDuration = 0.5f;
    float _timeUntilLastAttack;
    [SerializeField] float _attackRate = 0.08f;
    float _timeUntilNextAttackRate;
    bool _isAttacking;

    Vector3 _movement;
    Vector3 _shootDir;

    private Vector3 _mousePos;
    private Vector3 _direction;

	static Animator anim;
	public ParticleSystem particleSwipe;
	public AudioSource SlashSound;


    [Title ("Animations")]
    [SerializeField] Animator _animator;
    [HideInInspector] public float _velX;
    [HideInInspector] public float _velY;
    [HideInInspector] public bool _isMoving;
    [HideInInspector] public bool _justAttacked;

	void Start ()
	{
		anim = GetComponent<Animator> ();
	}

    void FixedUpdate ()
    {
        PlayerMovement ();
    }

    private void Update ()
    {
        PlayerAttack ();
    }

    private void PlayerMovement ()
    {
        _movement = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
        if (_movement.magnitude > _deadZone)
        {
            _isMoving = true;
			anim.SetBool("isRunning", true);
            Vector3 newMovement = _cameraOffset.forward * _movement.normalized.z + _cameraOffset.right * _movement.normalized.x;
            newMovement = Vector3.ClampMagnitude (newMovement, 1 / Mathf.Sqrt (2)) * Mathf.Sqrt (2);

            _rb.velocity = newMovement * _moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            _isMoving = false;
            _rb.velocity *= _decelerationRate * Time.fixedDeltaTime;
			anim.SetBool ("isRunning", false);
        }
        if (!_isAttacking && _rb.velocity.magnitude > _rotationDeadZone)
        {
            transform.rotation = Quaternion.LookRotation (_rb.velocity, Vector3.up);
        }

        _velX = _rb.velocity.x;
        _velY = _rb.velocity.y;
    }

    private void PlayerRotation ()
    {
        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
        Vector3 mouseOnScreen = (Vector3) Camera.main.ScreenToViewportPoint (Input.mousePosition);

        float angle = AngleBetweenTwoPoints (positionOnScreen, mouseOnScreen);

        Quaternion _newQuat = Quaternion.Euler (new Vector3 (0f, -angle - _rotationOffset, 0f));
        transform.rotation = Quaternion.Lerp (transform.rotation, _newQuat, Time.time * _rotSpeed);
    }

    private void PlayerAttack ()
    {
        if (_isAttacking && Time.time > _timeUntilLastAttack && _attackDuration != 0)
        {
            _isAttacking = false;
            _justAttacked = false;
            _timeUntilLastAttack = Time.time + 1 / _attackDuration;
        }

        if (Time.time > _timeUntilNextAttackRate && _attackRate != 0)
        {
            if (Input.GetMouseButtonDown (0))
            {
                _isAttacking = true;
                _justAttacked = true;
                AttackAbility ();
                _timeUntilNextAttackRate = Time.time + 1 / _attackRate;
				anim.SetTrigger ("isAttacking");
				SlashSound.Play ();

				//Debug.Log ("not working");



            }
        }
    }
    void AttackAbility ()
    {
		
        StartCoroutine (RotateForAttackDuration ());
    }



    IEnumerator RotateForAttackDuration ()
    {
		yield return new WaitForSeconds (0.3f);
		particleSwipe.Emit (1);

        while (_isAttacking)
        {
            PlayerRotation ();
            yield return null;
        }
    }

    float AngleBetweenTwoPoints (Vector3 a, Vector3 b)
    {
        return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}