using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInput : MonoBehaviour {
	TileHighlight tileHighlight;
	public enum states {idle, move};
	public static states userState;
	public Transform moveUnit;

	private Animator actionMenu;

	// Use this for initialization
	void Start () {
		userState = states.idle;
		tileHighlight = gameObject.GetComponent<TileHighlight>();
		actionMenu = GameObject.Find("Canvas/ActionMenu").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			mouseClick(new Vector2(mouseposition.x, mouseposition.y));
		}
	}

	void unitMove(Vector3 pos) {
		moveUnit.position = pos;
		Unit unitScript = moveUnit.GetComponent<Unit>();
		unitScript.status = 1;
		actionMenu.SetBool("isOpen", true);
	}

	public void attack() {
		WeaponType attactPattern = moveUnit.GetComponent<Unit>().weaponSets[0].getAttackPattern();
		tileHighlight.showAttackGrid(attactPattern.execute(moveUnit.position));
	}

	public void resumeIdle() {
		moveUnit.GetComponent<SpriteRenderer>().color= Color.gray;
		userState = states.idle;
		tileHighlight.highlightCtrl(tileHighlight.previousNodeList, true);
		actionMenu.SetBool("isOpen", false);
	}

	public void cancel() {
		moveUnit.position = tileHighlight.originTile;
		moveUnit.GetComponent<Unit>().status = 0;
		userState = states.idle;
		tileHighlight.highlightCtrl(tileHighlight.previousNodeList, true);
		actionMenu.SetBool("isOpen", false);
	}

	void clickAction(Vector2 point) {
		int layer = 1 << 8;
		Collider2D collider = Physics2D.OverlapPoint(point, layer);
		if (collider && collider.tag== "Player") {

		} else {
			unitMove( point);
			//resumeIdle();
		}
	}
	
	void mouseClick(Vector2	point) {
	  Collider2D[] collides =	Physics2D.OverlapPointAll(point);
		foreach (Collider2D collide in collides) {
			if (collide.tag == "GroundMove" && collide.GetComponent<gridHighlight>().canMove && userState == states.move) {
				clickAction(collide.transform.position);
			}

			if (collide.tag == "Player" && userState == states.idle && collide.GetComponent<Unit>().status == 0 ) {
				userState = states.move;
				moveUnit = collide.transform;
				tileHighlight.findHighlight(collide.transform.position, 4);
			}
		}
	}
}
