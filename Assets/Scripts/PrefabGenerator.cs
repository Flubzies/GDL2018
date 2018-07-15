using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGenerator : Singleton<PrefabGenerator>
{
	[SerializeField] List<PrefabProperties> _roomPrefabList;

	public Transform GetPrefab (RoomPrefabType _prefabType)
	{
		foreach (var item in _roomPrefabList)
		{
			if (item._roomPrefabType == _prefabType) return item._prefab;
		}
		return null;
	}

}

[System.Serializable]
public class PrefabProperties
{
	public RoomPrefabType _roomPrefabType;
	public Transform _prefab;
}