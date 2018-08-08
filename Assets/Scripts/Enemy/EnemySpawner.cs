using System.Collections;
using System.Collections.Generic;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] protected GameObject _enemyPrefab;
    [SerializeField] Vector2Int _minMaxEnemies;
    [SerializeField] protected float _spawnRadius;
    [SerializeField] float _ySpawnPos = 1.0f;
    [SerializeField][MinMaxSlider (1, 20, true)] Vector2 _spawnDelay;
    [SerializeField] BoxCollider _boxCollider;

    int _maxEnemies;
    Transform[] _listEnemies;
    Collider[] _colliders;

    void Start ()
    {
        _maxEnemies = Random.Range (_minMaxEnemies.x, _minMaxEnemies.y);
        _listEnemies = new Transform[_maxEnemies];
        StartCoroutine (SpawnEnemiesWithDelay ());
    }

    Vector3 GetSpawnLoc ()
    {
        Vector3 randPos = new Vector3 (Random.insideUnitSphere.x * _spawnRadius, _ySpawnPos, Random.insideUnitSphere.z * _spawnRadius);
        randPos += transform.position;
        _colliders = Physics.OverlapSphere (randPos, 1.5f);
        if (_colliders.Length == 0) return randPos;
        else return GetSpawnLoc ();
    }

    Transform CreateEnemy ()
    {
        Vector3 spawnPos = GetSpawnLoc ();
        return Instantiate (_enemyPrefab, spawnPos, Quaternion.identity).transform;
    }

    public void OnDeath ()
    {
        GameManager._Instance.OnSpawnerDeath ();
        _boxCollider.enabled = false;
        Destroy (this.gameObject, 0.1f);
    }

    IEnumerator SpawnEnemiesWithDelay ()
    {
        while (true)
        {
            yield return new WaitForSeconds (Random.Range (_spawnDelay.x, _spawnDelay.y));

            for (int i = 0; i < _maxEnemies; i++)
            {
                if (_listEnemies[i] == null)
                {
                    _listEnemies[i] = CreateEnemy ();
                    break;
                }
            }
        }
    }

    private void OnValidate ()
    {
        if (_minMaxEnemies.x > _minMaxEnemies.y)
            _minMaxEnemies.x = _minMaxEnemies.y;
    }
}