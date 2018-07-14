using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] int _damage = 1;
	[SerializeField] float _speed = 40.0f;
	[SerializeField] Rigidbody _rb;
	Health _health = null;

	public void FixedUpdate ()
	{
		// if(_rb.velocity.magnitude )
	}

	public void SetVelocity (Vector3 veloctiy_)
	{
		_rb.velocity = veloctiy_.normalized * _speed;
	}

	public void SetDamage (int damage_)
	{
		_damage = damage_;
	}

	private void OnCollisionEnter (Collision other)
	{
		_health = other.gameObject.GetComponent<Health> ();
		if (_health != null)
		{
			_health.Damage (_damage);
		}

		SafeDestroy.DestroyGameObject (this);
	}
}