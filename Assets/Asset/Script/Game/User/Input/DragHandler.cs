using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Utility;

public class DragHandler : MonoBehaviour {

	public virtual void OnDragBegin(GameObject p_gameobject) {
		// if (p_unit == null) return;

		// moveUnit = p_unit;
		// inputManager.DisplayRoute(moveUnit);
	}

	//Count as onDrag
	public virtual void OnDrag( Vector3 p_mousePosition ) {
		// if (moveUnit == null) return; 

		// //Move Friendly Unit
		// inputManager.PathTracking( moveUnit );
		// moveUnit.transform.position = new Vector2( p_mousePosition.x, p_mousePosition.y);

	}

	//onDragUp
	public virtual void OnDrop(Vector3 p_mousePosition) {
		// inputManager.MoveUnitOnDrop(moveUnit , p_mousePosition);
		// moveUnit = null;
	}
}
