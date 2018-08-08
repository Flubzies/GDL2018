using System.Collections;
using System.Collections.Generic;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : SemiSingleton<LevelGenerator>
{

	public enum SpaceType
	{
		Vacant,
		Blocked,
		Occupied,
		Start
	}

	int[, ] _levelMatrix;

	[Title ("Level Generation")]

	[SerializeField] int _gridSize;
	[SerializeField] int _roomSize;
	[SerializeField] float _minRoomCount;
	[SerializeField] int _maxFailCount = 5;

	[Title ("Room Generation")]

	[SerializeField] List<Room> _roomPrefabs;
	[Tooltip ("Must be Up Down Left Right")]
	[SerializeField] List<Transform> _wallpoints;
	[SerializeField] LayerMask _wallsLayerMask;

	[SerializeField] PrefabGenerator _prefabGen;
	[SerializeField] NavMeshSurface _navMesh;
	[SerializeField] Transform _player;
	[SerializeField] Transform _cameraOffset;

	[Title ("Generate")]
	[SerializeField] bool _generateOnAwake;
	[SerializeField] bool _isEditMode;
	[SerializeField] bool _openMenuOnStart;
	[SerializeField] bool _debug;

	bool _playerSpawned = false;
	Vector3 _playerSpawnLocation;
	Collider[] _colliders;

	int _roomCount;
	int _failCount;
	bool _generateLevel;

	void Awake ()
	{
		if (_generateOnAwake)
		{
			DestroyAllChildren ();
			ResetMatrix ();
			GenerateMatrix ();
			GenerateLevel ();
		}
		if (_generateOnAwake) GenerateNewLevel ();
	}

	[ButtonGroup ("Level Generator", 0)]
	void DestroyAllChildren ()
	{
		if (_isEditMode)
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				SafeDestroy.DestroyGameObject (transform.GetChild (0));
			}
		}
		else
		{
			foreach (Transform tran in transform)
			{
				Destroy (tran.gameObject);
			}
		}

	}

	[ButtonGroup ("Level Generator", 0)]
	void ResetMatrix ()
	{
		_levelMatrix = new int[_gridSize, _gridSize];

		for (int x = 0; x < _gridSize; x++)
		{
			for (int y = 0; y < _gridSize; y++)
			{
				if (x == 0 || y == 0 || x == _gridSize - 1 || y == _gridSize - 1) _levelMatrix[x, y] = (int) SpaceType.Blocked;
				else _levelMatrix[x, y] = (int) SpaceType.Vacant;
			}
		}
	}

	[ButtonGroup ("Level Generator", 1)]
	void GenerateMatrix ()
	{
		Vector2Int currentPos = new Vector2Int (_gridSize / 2, _gridSize / 2);
		_levelMatrix[currentPos.x, currentPos.y] = (int) SpaceType.Start;

		_roomCount = 1;
		_generateLevel = false;

		for (int i = 0; i < _minRoomCount; i++)
		{
			if (_generateLevel) break;
			AddRoomToMatrix (currentPos);
		}

		if (_debug) Debug.Log ("Matrix Generation Complete. Room Count: " + _roomCount);
	}

	void AddRoomToMatrix (Vector2Int curPos_)
	{
		curPos_ = GetAdjacentPos (new Vector2Int (curPos_.x, curPos_.y));

		if (curPos_ == Vector2Int.zero) Debug.LogError ("This has got to be bad.");

		if (_levelMatrix[curPos_.x, curPos_.y] == (int) SpaceType.Vacant)
		{
			_levelMatrix[curPos_.x, curPos_.y] = (int) SpaceType.Occupied;
			_roomCount++;
		}
		else
		{
			if (_failCount == _maxFailCount) _generateLevel = true;
			_failCount++;
			AddRoomToMatrix (curPos_);
		}
	}

	[ButtonGroup ("Level Generator", 2)]
	void GenerateLevel ()
	{
		if (_debug) Debug.Log ("Generating Level");
		Room tempRoom;
		RoomPrefabType rpt;
		Transform trans;
		int spawnerCount = 0;

		for (int x = 0; x < _gridSize; x++)
		{
			for (int y = 0; y < _gridSize; y++)
			{
				if (_levelMatrix[x, y] == (int) SpaceType.Occupied || _levelMatrix[x, y] == (int) SpaceType.Start)
				{
					tempRoom = Instantiate (_roomPrefabs.GetRandomFromList (), MatrixToGridCoordinates (x, y), Quaternion.identity, transform);
					RandYRot (tempRoom.transform);
					rpt = tempRoom._roomPrefabType;

					if (!_playerSpawned)
					{
						_cameraOffset.position = _player.transform.position = tempRoom._spawnLocation.position;
						_playerSpawned = true;
					}
					else
					{
						if (Random.value > 0.5f) continue;

						if (rpt != RoomPrefabType.Undefined)
						{
							trans = _prefabGen.GetPrefab (rpt);
							Instantiate (trans, tempRoom._spawnLocation.position, Quaternion.identity, tempRoom.transform);
							if (rpt == RoomPrefabType.EnemySpawner) spawnerCount++;
						}
					}

				}
			}
		}

		GameManager._Instance.SetSpawnerCount (spawnerCount);
	}

	protected void RandYRot (Transform trans_)
	{
		int x = Random.Range (0, 4);
		trans_.rotation = Quaternion.AngleAxis (x * 90, Vector3.up);
	}

	[Button (ButtonSizes.Large)]
	void GenerateNewLevel ()
	{
		DestroyAllChildren ();
		ResetMatrix ();
		GenerateMatrix ();
		GenerateLevel ();
		DestroyWalls ();

		if (!_isEditMode)
		{
			StartCoroutine (GenNavMesh ());
		}
		else
		{
			_navMesh.RemoveData ();
			_navMesh.BuildNavMesh ();
		}
	}

	IEnumerator GenNavMesh ()
	{
		if (_openMenuOnStart) SettingsMenu._Instance.OpenMenu ();
		yield return new WaitForSeconds (1.0f);
		_navMesh.RemoveData ();
		_navMesh.BuildNavMesh ();
	}

	[ButtonGroup ("Level Generator", 4)]
	void DestroyWalls ()
	{
		if (_debug) Debug.Log ("Destroying walls.");

		int walls = 4;
		int adjRooms;

		Vector2Int pos = Vector2Int.zero;
		bool[] wallPosition = new bool[4];
		Vector2Int tempVec;

		for (int x = 0; x < _gridSize; x++)
		{
			for (int y = 0; y < _gridSize; y++)
			{
				if (_levelMatrix[x, y] == (int) SpaceType.Vacant || _levelMatrix[x, y] == (int) SpaceType.Blocked) continue;

				pos = new Vector2Int (x, y);
				adjRooms = 0;
				wallPosition = new bool[4];

				for (int i = 0; i < walls; i++)
				{
					tempVec = GetAdjacentPos (pos, i);
					if (_levelMatrix[tempVec.x, tempVec.y] == (int) SpaceType.Occupied || _levelMatrix[tempVec.x, tempVec.y] == (int) SpaceType.Start)
					{
						wallPosition[i] = true;
						adjRooms++;
					}
					else wallPosition[i] = false;
				}

				// We now have the adjacent room count 
				// And the positions of the walls.

				DestroyWallsAt (wallPosition, adjRooms, MatrixToGridCoordinates (x, y));
			}
		}
		if (_debug) Debug.Log ("Completed");
	}

	void DestroyWallsAt (bool[] wallPositions_, int adjRooms_, Vector3 roomPos_)
	{
		int wallsToDelete = 1;
		int deletedCount = 0;
		int[] randomizedArray = new int[] { 0, 1, 2, 3 };

		switch (adjRooms_)
		{
			case 1:
				wallsToDelete = 1;
				break;
			case 2:
				wallsToDelete = 2;
				break;
			case 3:
				wallsToDelete = Random.Range (1, 3);
				break;
			case 4:
				wallsToDelete = Random.Range (1, 4);
				break;
		}

		randomizedArray.Shuffle ();

		// number of collisions 
		for (int i = 0; i < wallPositions_.Length; i++)
		{

			// generate a random number from 1-4 this is the random wall to be destroyed.
			// randIndex must not be < i
			// randIndex has to be < wallPositions_.Length
			// randIndex must not be equal to wallPos[i] == false
			// if randIndex is == wallPos[i] == false && wallPositions_.Length
			// then exit the loop
			// randIndex

			// 1 walls to delete so we have to do this for loop once at a random index
			// 2 walls to delete 2 walls at 2 random idicies.

			if (deletedCount < wallsToDelete)
				if (wallPositions_[randomizedArray[i]])
				{
					// These wall can be deleted if wanted.

					_wallpoints[randomizedArray[i]].transform.position = roomPos_;
					DestroyWall (_wallpoints[randomizedArray[i]].GetChild (0).transform.position);
					DestroyWall (_wallpoints[randomizedArray[i]].GetChild (1).transform.position);
					_wallpoints[randomizedArray[i]].transform.position = Vector3.zero;
					deletedCount++;
				}

		}
	}

	void DestroyWall (Vector3 pos_)
	{
		_colliders = Physics.OverlapSphere (pos_, 0.2f, _wallsLayerMask);

		if (_isEditMode)
		{
			for (int i = 0; i < _colliders.Length; i++)
			{
				SafeDestroy.DestroyGameObject (_colliders[i]);
			}
		}
		else
		{
			foreach (Collider col in _colliders)
			{
				Destroy (col.gameObject);
			}
		}
	}

	Vector3 MatrixToGridCoordinates (int x_, int y_)
	{
		return new Vector3 (x_ * ((float) _roomSize / 2.0f), 0, y_ * (float) _roomSize / 2.0f);
	}

	Vector2Int GetAdjacentPos (Vector2Int pos_, int dir_ = -1)
	{
		if (dir_ == -1) dir_ = Random.Range (0, 4);
		Vector2Int endPos_ = Vector2Int.zero;

		switch (dir_)
		{
			case 0:
				endPos_ = pos_ + Vector2Int.up;
				return endPos_;
			case 1:
				endPos_ = pos_ + Vector2Int.down;
				return endPos_;
			case 2:
				endPos_ = pos_ + Vector2Int.left;
				return endPos_;
			case 3:
				endPos_ = pos_ + Vector2Int.right;
				return endPos_;
		}

		return endPos_;
	}

	private void OnValidate ()
	{

	}

}