using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class DatabaseManager : MonoBehaviour {

	public ScriptableListPrefab levelObjects;
	// public CharacterListPrefab characterObjects;

	// public LevelPrefab FindLevel(int p_level) {
	// 	// return levelObjects.prefabList.Find(x=>x._level == p_level);
	// }

	public T FindByID<T>(string p_id) where T : ScriptablePrefab
    {
        for (int i = 0; i < levelObjects.prefabList.Count; i++) if (levelObjects.prefabList[i]._id == p_id) return (T)levelObjects.prefabList[i];
        Debug.Log("FindByID [" + p_id + "] not found!");
        return default(T);
    }

	public List<T> FindAllByType<T>() where T : ScriptablePrefab
    {	
		List<T> list = new List<T>();
        for (int i = 0; i < levelObjects.prefabList.Count; i++) if (levelObjects.prefabList[i].GetType() == typeof(T)) list.Add( (T)levelObjects.prefabList[i] );
        return list;
    }
}