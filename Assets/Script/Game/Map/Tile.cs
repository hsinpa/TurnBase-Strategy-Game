using UnityEngine;
using System.Collections;


[System.Serializable]
public class Tile {
	public JSONObject jsonType;
	public bool walkable;
	public Vector2 position;
	public int gridX;
	public int gridY;
	public int gCost;
	public int hCost;


	public int defenseBonus = 0;
	public int cost = -1;

	public Tile parent;
	
	
	public Tile(Vector3 _pos, int _gridX, int _gridY, JSONObject _type) {
		//walkable = _walkable;
		position = _pos;
		gridX = _gridX;
		gridY = _gridY;

		UpdateInfo(_type);
	}
	
	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public void UpdateInfo(JSONObject _type) {
		jsonType = _type;

		if (_type.HasField("cost")) cost = (int)_type.GetField("cost").n;
		if (_type.HasField("defense")) defenseBonus = (int)_type.GetField("defense").n;

		walkable = (cost > 0);
	}
}
