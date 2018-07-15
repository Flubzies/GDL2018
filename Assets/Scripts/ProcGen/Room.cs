using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

	[Tooltip ("Chance for the room to spawn from 1.0 to 0.0 where 1.0 is 100%")]
	public float _roomRarity = 1.0f;
	public RoomPrefabType _roomPrefabType;
	public Transform _spawnLocation;

	public float GetRoomRarity ()
	{
		return _roomRarity;
	}

}

public enum RoomPrefabType
{
	Undefined,
	HealthUpgrade,
	SpeedUpgrade,
	EnemySpawner
}