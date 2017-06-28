using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ObserverPattenr;

public class GameUIManager : Observer{
	GameManager gameManager;
	public Animator actionMenu { get { return transform.Find("ActionMenu").GetComponent<Animator>(); } }

	public TopInfoView topInfoView { get { return transform.Find("infotab_top").GetComponent<TopInfoView>(); } }

	public void SetUp() {
		gameManager = MainApp.Instance.game;
	}

	public override void OnNotify (string p_event, params object[] p_objects) {
		switch(p_event) {
			case EventFlag.GameUI.SetUp :
				SetUp( );
			break;
		}
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
		// gameManager.inputManager.moveUnit.transform.position = gameManager.map.gridManager.originTile;
		gameManager.inputManager.moveUnit.status = Unit.Status.Idle;
		gameManager.inputManager.FreePanel();
	}

}
