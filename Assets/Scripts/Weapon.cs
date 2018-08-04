using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	Health h;
	public int _Damage { get; set; }

	private void OnTriggerEnter (Collider other)
	{
		h = other.gameObject.GetComponent<Health> ();
		if (h != null) h.Damage (_Damage);
	}
}