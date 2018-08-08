using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	int _health;
	bool _isDead;

	[SerializeField]
	[Range (1, 1000)]
	[Tooltip ("The Initial Health of the GameObject.")]
	private int _initialHealth;

	[SerializeField]
	[Range (1, 1000)]
	[Tooltip ("The Maximum Health of the GameObject.")]
	private int _maxHealth;
	public int _GetMaxHealth { get { return _maxHealth; } }

	[SerializeField]
	[Tooltip ("Show Health Debug Logs in console.")]
	private bool _debug;

	[SerializeField] bool _invulnerableAfterDamage;
	[ShowIf ("_invulnerableAfterDamage")]
	[SerializeField] TimedEvent _invulnerabilityEvent;
	private bool _invulnerable;

	public UnityEvent OnDeath, OnDamaged, OnBecomeInvulnerable, OnEndInvulnerable;
	bool _onDeathInvoked;

	public float _GetHealthPercent
	{
		get
		{
			if (_maxHealth != 0) return (float) _health / (float) _maxHealth;
			else Debug.LogError ("Max Health is 0!");
			return 0;
		}
	}

	void Start ()
	{
		ResetVariables ();
	}

	/// <summary>
	/// Always adds to the health. Use Damage to take away.
	/// </summary>
	public void Heal (int healValue_)
	{
		_health += Mathf.Abs (healValue_);
		if (_health > _maxHealth) _health = _maxHealth;
		if (_debug) Debug.Log (gameObject.name + " healed: " + healValue_ + " | Health: " + _health);
	}

	/// <summary>
	/// Always takes away from the health. Use Heal to add.
	/// </summary>
	public void Damage (int damageValue_)
	{
		if (_invulnerable) return;
		_health -= Mathf.Abs (damageValue_);
		if (_health < 0) _health = 0;
		if (_debug) Debug.Log (gameObject.name + " damaged: " + damageValue_ + " | Health: " + _health);
		OnDamaged.Invoke ();
		if (_invulnerableAfterDamage)
		{
			OnBecomeInvulnerable.Invoke ();
			_invulnerable = true;
			_invulnerabilityEvent.TriggerEvent (this);
			_invulnerabilityEvent.OnEventComplete (OnEndInvulnerablity);
		}
		if (IsDead () && !_onDeathInvoked)
		{
			_onDeathInvoked = true;
			OnDeath.Invoke ();
		}
	}

	public void Damage (int damageValue_, Transform damager_, bool knockback_)
	{
		// Add knockback here
	}

	void OnEndInvulnerablity ()
	{
		_invulnerable = false;
		OnEndInvulnerable.Invoke ();
	}

	/// <summary>
	/// Returns true if the health is less than or equal to 0.
	/// </summary>
	public bool IsDead ()
	{
		if (_health <= 0) _isDead = true;
		return _isDead;
	}

	public void ResetVariables ()
	{
		_isDead = false;
		_health = _initialHealth;
		_onDeathInvoked = false;
	}

	private void OnValidate ()
	{
		if (_initialHealth <= 0) _initialHealth = 1;
		if (_maxHealth < _initialHealth) _maxHealth = _initialHealth;
	}

}