using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
	public EnemyAI _enemyAI;
	[SerializeField] float _waitTime = 1.0f;

	private void OnTriggerStay (Collider other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			_enemyAI.StartChasingPlayer (other.transform);
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			StartCoroutine (StopFollow ());
		}
	}

	IEnumerator StopFollow ()
	{
		yield return new WaitForSeconds (_waitTime);
		_enemyAI.StopChasingPlayer ();
	}

}