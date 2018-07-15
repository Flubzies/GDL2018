using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform _enemyPrefab;
    [SerializeField] int _maxEnemies;
    List<GameObject> _enemyPrefabs = new List<GameObject> ();
    [SerializeField] float _spawnEnemiesEvery;

    void Start ()
    {
        InitialSpawn ();
        StartCoroutine (ActivateEnemy ());
    }

    void InitialSpawn ()
    {
        for (int i = 0; i < _maxEnemies; i++)
        {
            _enemyPrefabs.Add (Instantiate (_enemyPrefabs[i], Random.insideUnitCircle, Quaternion.identity));
            _enemyPrefabs[i].SetActive (false);
        }
    }

    IEnumerator ActivateEnemy ()
    {
        Debug.Log (_enemyPrefabs.Count);
        while (true)
        {
            for (int i = 0; i < _enemyPrefabs.Count; i++)
            {
                if (_enemyPrefabs[i].activeSelf == false)
                {
                    _enemyPrefabs[i].SetActive (true);
                    Debug.Log ("Did it");
                }
            }

            yield return new WaitForSeconds (_spawnEnemiesEvery);
        }
    }
}