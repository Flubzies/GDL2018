using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] protected GameObject _enemyPrefab;
    [SerializeField] protected int _maxEnemies;
    protected List<GameObject> _listEnemies;

    [SerializeField] protected float _activateDelay;
    [SerializeField] protected float _spawnRadius;

    void Start ()
    {
        _listEnemies = new List<GameObject> ();
        StartCoroutine (CreateEnemies ());
        StartCoroutine (SpawnEnemiesWithDelay ());
    }

    Vector3 SpawnLoc ()
    {
        Vector3 _randomSpawn = new Vector3 (Random.insideUnitSphere.x * _spawnRadius, 1, Random.insideUnitSphere.z * _spawnRadius);
        return transform.position + _randomSpawn;
    }

    void CreateEnemy ()
    {
        SpawnLoc ();
        GameObject enemy = (GameObject) Instantiate (_enemyPrefab, SpawnLoc (), Quaternion.identity, transform);
        _listEnemies.Add (enemy);
        enemy.SetActive (false);
    }

    IEnumerator CreateEnemies ()
    {
        while (_listEnemies.Count != _maxEnemies)
        {
            yield return new WaitForSeconds (_activateDelay / 2.0f);
            CreateEnemy ();
        }
    }

    IEnumerator SpawnEnemiesWithDelay ()
    {
        while (true)
        {
            yield return new WaitForSeconds (_activateDelay);

            for (int i = 0; i < _maxEnemies; i++)
            {
                if (_listEnemies[i] == null) continue;

                if (_listEnemies[i].activeSelf == false)
                {
                    _listEnemies[i].transform.position = SpawnLoc ();
                    _listEnemies[i].SetActive (true);
                    break;
                }
            }
        }
    }
}