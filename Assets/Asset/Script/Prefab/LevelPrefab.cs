using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelPrefab : ScriptableObject {
	//General
	public int _level;

	public List<MapPrefab> _mapList;
}