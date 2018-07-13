using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelGenerator : SerializedMonoBehaviour
{

	public enum SpaceType
	{
		Vacant,
		Blocked,
		Occupied,
		Start
	}

	[ReadOnly][TableMatrix (SquareCells = true)]
	public int[, ] _levelMatrix;

	[SerializeField] int _gridSize;
	[SerializeField] int _roomSize;
	[SerializeField] float _minRoomCount;
	[SerializeField] int _maxFailCount = 5;

	[SerializeField] List<Room> _roomPrefabs;
	[Tooltip ("Must be Up Down Left Right")]
	[SerializeField] List<Transform> _wallpoints;

	int _roomCount;
	int _failCount;
	bool _generateLevel;

	[ButtonGroup ("Level Generator", 0)]
	void DestroyAllChildren ()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			SafeDestroy.DestroyGameObject (transform.GetChild (0));
		}
	}

	[ButtonGroup ("Level Generator", 0)]
	void ResetMatrix ()
	{
		_levelMatrix = new int[_gridSize, _gridSize];

		// for (int x = 0; x < _gridSize; x++)
		// {
		// 	for (int y = 0; y < _gridSize; y++)
		// 	{
		// 		_levelMatrix[x, y] = new int ();
		// 	}
		// }

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

		// GenerateLevel();
		// After that generate the corridors.

		Debug.Log ("Matrix Generation Complete. Room Count: " + _roomCount);
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
		Debug.Log ("Generating Level");
		Vector3 spawnPoint;
		DestroyAllChildren ();

		for (int x = 0; x < _gridSize; x++)
		{
			for (int y = 0; y < _gridSize; y++)
			{
				if (_levelMatrix[x, y] == (int) SpaceType.Occupied)
				{
					spawnPoint = new Vector3 (x * ((float) _roomSize / 2.0f), 0, y * (float) _roomSize / 2.0f);
					Instantiate (_roomPrefabs.GetRandomFromList (), spawnPoint, Quaternion.identity, transform);
				}
			}
		}
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