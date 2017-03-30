using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Utility;

public class DragHandler : MonoBehaviour {

	public enum States { Idle, Move, WaitForAction, Attack, Drag };
    public States inputState;

	//Distinguish between click and drag
    protected float mouseClickDistance = 0;
    protected float isDragDistance = 0.05f;

    protected Vector3 startClickPoint;

	public void OnUpdate() {
		Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

		//Mouse CLick
		if (Input.GetMouseButton(0))
		{
			if (startClickPoint == Vector3.zero) startClickPoint = mouseposition;

			//Check if it meet the condition of dragging
			if (inputState != States.Drag)
			{
				float dis = Vector3.Distance(mouseposition, startClickPoint);
				if (dis > isDragDistance)
				{
					GameObject touchedUnit = GetUnitByMousePos(mouseposition);
					if (touchedUnit != null)
					{
						//onDragBegin
						OnDragBegin(touchedUnit);
					}
				}
			}

			//onDrag
			if (inputState == States.Drag) OnDrag(mouseposition);
		}

		//Drop
		if (Input.GetMouseButtonUp(0) && inputState == States.Drag)
		{
			OnDrop(mouseposition);
		}
	}

	public virtual void OnDragBegin(GameObject p_gameobject) {
		inputState = States.Drag;
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
		startClickPoint = Vector3.zero;
		inputState = States.Idle;
	}

	private GameObject GetUnitByMousePos(Vector3 p_mousePosition) {
        int u_layer = GeneralSetting.unitLayer;

        //Get most top sorting collider
        Collider2D mCollide = Physics2D.OverlapPoint(p_mousePosition, u_layer);
        if (mCollide == null) return null;

        return mCollide.gameObject;
    }
}
