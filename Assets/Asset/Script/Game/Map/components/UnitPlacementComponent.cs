using UnityEngine;
using System.Collections;
using Player;


//Placement type such as Unit, Spawnpoint
[System.Serializable]
public class UnitPlacementComponent {
	public EventFlag.UserType userType;
	public EventFlag.PlacementType placementType;
	public JSONObject propertyJSON;
	public Vector2 position;

	public UnitPlacementComponent(EventFlag.UserType p_userType, EventFlag.PlacementType p_placementType,Vector2 p_position, JSONObject p_propertyJSON) {
		userType = p_userType;
		position = p_position;
		propertyJSON = p_propertyJSON;
		placementType = p_placementType;
	}

}
