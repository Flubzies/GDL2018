using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] LayerMask _layerToDamage;
	bool _weaponEnabled;

	Health h;
	public int _Damage { get; set; }

	public void EnableWeapon ()
	{
		_weaponEnabled = true;
	}

	public void DisableWeapon ()
	{
		_weaponEnabled = false;
	}

	private void OnTriggerEnter (Collider other)
	{
		if (!_weaponEnabled) return;
		if (_layerToDamage.IsInLayerMask (other.gameObject))
		{
			h = other.gameObject.GetComponent<Health> ();
			if (h != null)
			{
				h.Damage (_Damage);
			}
		}
	}
}