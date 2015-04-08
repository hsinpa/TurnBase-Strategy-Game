using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInput : MonoBehaviour {
	TileHighlight tileHighlight;
	public enum states {idle, move};
	public static states userState;
	public Transform moveUnit;

	// Use this for initialization
	void Start () {
		userState = states.idle;
		tileHighlight = gameObject.GetComponent<TileHighlight>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			mouseClick(new Vector2(mouseposition.x, mouseposition.y));
		}
	}

	void unitMove(Transform unit, Vector3 pos) {
		unit.position = pos;
	}

	void resumeIdle() {
		userState = states.idle;
		tileHighlight.highlightCtrl(tileHighlight.finalNodeList, true);
	}
	
	void mouseClick(Vector2	point) {
	  Collider2D[] collides =	Physics2D.OverlapPointAll(point);
		foreach (Collider2D collide in collides) {
			if (collide.tag == "Player") {
				userState = states.move;
				moveUnit = collide.transform;
				tileHighlight.findHighlight(collide.transform.position, 4);
			}

			if (collide.tag == "Ground" && collide.GetComponent<gridHighlight>().canMove && userState == states.move) {
				unitMove(moveUnit, collide.transform.position);
				resumeIdle();
			}
		}
	}
}
