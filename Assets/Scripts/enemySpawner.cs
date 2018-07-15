using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] protected List <GameObject> _enemyPrefabs;
    [SerializeField] protected float _activateDelay;
    [SerializeField] protected Transform _spawnLocation;

    void Start ()
    {
        initialSpawn();

        StartCoroutine(activateEnemy());
    }
	
	void Update ()
    {

    }

    void initialSpawn()
    {
        for (int i = 0; i < _enemyPrefabs.Count; i++)
        {
            Instantiate(_enemyPrefabs[i], _spawnLocation.position, Quaternion.identity);
            _enemyPrefabs[i].SetActive(false);

        }
    }

    IEnumerator activateEnemy()
    {
        Debug.Log(_enemyPrefabs.Count);
        while(true)
        {

            for (int i = 0; i < _enemyPrefabs.Count; i++)
            {
                if (_enemyPrefabs[i].activeSelf == false)
                {
                    _enemyPrefabs[i].SetActive(true);
                    Debug.Log("Did it");
                }

            }

            yield return new WaitForSeconds(_activateDelay);
        }
    }
}
