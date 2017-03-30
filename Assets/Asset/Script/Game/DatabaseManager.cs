using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class DatabaseManager : MonoBehaviour {

	public LevelListPrefab levelObjects;
	public CharacterListPrefab characterObjects;

	public LevelPrefab FindLevel(int p_level) {
		return levelObjects.levelList.Find(x=>x._level == p_level);
	}
}