using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] protected GameObject _enemyPrefab;
    [SerializeField] protected int _maxEnemies;
    protected List<GameObject> _listEnemies;

    [SerializeField] protected float _activateDelay;
    [SerializeField] protected Transform _spawnArea;
    [SerializeField] protected float _spawnRadius;
    protected Vector3 _spawnLocation;

    void Start ()
    {
        _listEnemies = new List<GameObject>();

        initialSpawn();

        StartCoroutine(activateEnemy());
    }
	
	void createSpawnLocation()
    {
        Vector3 _randomSpawn = new Vector3(Random.insideUnitSphere.x * _spawnRadius, transform.position.y, Random.insideUnitSphere.z * _spawnRadius);
        _spawnLocation = _spawnArea.position + _randomSpawn;
    }

    void initialSpawn()
    {

        for (int i = 0; i < _maxEnemies; i++)
        {
            createSpawnLocation();
            GameObject _enmey = (GameObject)Instantiate(_enemyPrefab, _spawnLocation, Quaternion.identity);
            _listEnemies.Add(_enmey);
            _enmey.SetActive(false);

        }
    }

    IEnumerator activateEnemy()
    {
        while(true)
        {
            for (int i = 0; i < _maxEnemies; i++)
            {
                if (_listEnemies[i].activeSelf == false)
                {
                    createSpawnLocation();
                    _listEnemies[i].transform.position = _spawnLocation;
                    _listEnemies[i].SetActive(true);
                    break;
                }
            }

            yield return new WaitForSeconds(_activateDelay);
        }
    }
}
