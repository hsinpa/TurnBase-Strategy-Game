using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using Utility;
using Player;

public class InputManager : MonoBehaviour {
	public enum States {Idle, Move, WaitForAction, Attack};
	public States inputState;
	public Unit moveUnit;

	private List<Tile> recordTile = new List<Tile>();
	GameManager gameManager;
	GameUIHandler gameUI;
	Map _Map;

	User player { get {return transform.FindChild("player").GetComponent<User>();  } }

	// Use this for initialization
	void Start () {
		inputState = States.Idle;
		gameManager = GameObject.Find("game").GetComponent<GameManager>();

		_Map = gameManager.map;

		gameUI = GameObject.Find("ui").GetComponentInChildren<GameUIHandler>();
	}


	//========================================================= General Input Command =========================================================

	public void FreePanel() {
		inputState = InputManager.States.Idle;
		_Map.gridManager.highlightCtrl(_Map.grids, true);
		gameUI.actionMenu.SetBool("isOpen", false);
	}

	void PathTracking() {
			Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			Collider2D collide = Physics2D.OverlapPoint(new Vector2(mouseposition.x, mouseposition.y));
				
			if (collide == null || collide.tag != "GroundMove" ) return;

			List<Tile> tiles = FindPath(moveUnit.transform.position, collide.transform.position);
			recordTile = tiles;

			List<Vector2> pathList = tiles.Select(x=> x.position).ToList();
			pathList.Insert(0, moveUnit.unitPos);
			_Map.gridManager.DrawPathLine(pathList);
	}

	public List<Tile> FindPath(Vector2 startPos, Vector2 endPos) {
		GridHolder startGrid = _Map.FindTileByPos(startPos);
		GridHolder endGrid = _Map.FindTileByPos(endPos);
		return _Map.gridManager.aPathFinding.FindPath(startGrid, endGrid);
	}

	public void MoveUnit( Unit p_moveunit,  List<Tile> tiles, System.Action callback = null) {
		if (tiles.Count <= 0) return;
		Vector3[] path = tiles.ConvertAll<Vector3>(x => x.position).ToArray();

		p_moveunit.Move(path, delegate() {
			//Clear Path Line
			_Map.gridManager.DrawPathLine(new List<Vector2>());
			if (callback != null) callback();
		});
	}

	// ========================================================= Player Input Command =========================================================

	void Update () {
		if (gameManager.currentUser != gameManager.player) return;

		//Mouse CLick
		if (Input.GetMouseButtonUp(0)) {
			Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			MouseClickHandler(new Vector2(mouseposition.x, mouseposition.y) );
		} 

		//Mouse Position Tracking
		switch (inputState) {
			case States.Move:
				PathTracking();
			break;
		}
	}

	void MouseClickHandler(Vector2	point) {
		int UT_layer = GeneralSetting.unitLayer + GeneralSetting.terrainLayer;
		//Get most top sorting collider
		List<Collider2D> collides = Physics2D.OverlapPointAll(point, UT_layer).ToList();
		collides.Sort((x, y) => x.GetComponent<SpriteRenderer>().sortingOrder.CompareTo(y.GetComponent<SpriteRenderer>().sortingOrder));
		collides.Reverse();
		if (collides == null || collides.Count <= 0) return; 
		Collider2D mCollide = collides[0];

		Debug.Log(mCollide.tag);

		switch (inputState) {
			case States.Idle :
				//Move Friendly Unit
				if (mCollide.tag == "Player" && mCollide.GetComponent<Unit>().status == 0 ) {
					inputState = States.Move;
					moveUnit = mCollide.GetComponent<Unit>();
					_Map.gridManager.FindPossibleRoute(moveUnit, gameManager.enemy);
				}
			break;

			case States.Move :
				if (mCollide.tag == "GroundMove" && mCollide.GetComponent<GridHolder>().canMove) {
					inputState = States.WaitForAction;
					MoveUnit(moveUnit, recordTile);
					gameUI.actionMenu.SetBool("isOpen", true);
				}

			break;

			case States.Attack :
				if (mCollide.tag == "Enemy") {
					Unit target = mCollide.GetComponent<Unit>();
					GridHolder gridHolder = _Map.FindTileByPos(target.unitPos);
					moveUnit.Attack( target, gridHolder);
					moveUnit.status = Unit.Status.Rest;
					FreePanel();
				}
			break;
		}
	}
}
