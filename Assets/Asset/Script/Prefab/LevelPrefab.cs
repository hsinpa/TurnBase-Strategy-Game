using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPrefab : ScriptablePrefab {
	//General
	public int _level;
	public int _average_enemy_level;
	public int _level_range;

	public List<MapPrefab> _mapList = new List<MapPrefab>();
}