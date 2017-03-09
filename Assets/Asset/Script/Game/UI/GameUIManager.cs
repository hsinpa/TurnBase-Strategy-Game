using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ObserverPattenr;

public class GameUIManager : Observer{
	GameManager gameManager;
	public Animator actionMenu { get { return transform.Find("ActionMenu").GetComponent<Animator>(); } }

	public void SetUp(MainApp p_main) {
		gameManager = p_main.game;
	}

	public override void OnNotify (string p_event, params object[] p_objects) {
		
	}

	public void EndTurnClick() {
		gameManager.EndTurn();
	}


	public void AttackClick() {
		gameManager.map.gridManager.ResetGrid(gameManager.map.grids);

		gameManager.map.gridManager.FindAttackGrid( gameManager.inputManager.moveUnit.GetComponent<Unit>());

		gameManager.inputManager.inputState = InputManager.States.Attack;
		
	}

	public void ConfirmClick() {
		gameManager.inputManager.moveUnit.status = Unit.Status.Moved;
		gameManager.inputManager.moveUnit.UnitHighLightControl(false);
		gameManager.inputManager.FreePanel();
	}

	public void ResumeClick() {
		gameManager.inputManager.moveUnit.transform.position = gameManager.map.gridManager.originTile;
		gameManager.inputManager.moveUnit.status = Unit.Status.Idle;
		gameManager.inputManager.FreePanel();
	}

}
