using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIHandler : MonoBehaviour {
	GameManager gameManager;
	public Animator actionMenu { get { return GetComponent<Animator>(); } }

	void Start() {
		gameManager = GameObject.Find("game").GetComponent<GameManager>();
	}

	public void EndTurnClick() {
		gameManager.EndTurn();
	}


	public void AttackClick() {
		gameManager.map.gridManager.showAttackGrid( gameManager.inputManager.moveUnit.GetComponent<Unit>());
		gameManager.inputManager.inputState = InputManager.States.Attack;
	}

	public void ConfirmClick() {
		gameManager.inputManager.moveUnit.status = Unit.Status.Rest;
		gameManager.inputManager.FreePanel();
	}

	public void ResumeClick() {
		gameManager.inputManager.moveUnit.transform.position = gameManager.map.gridManager.originTile;
		gameManager.inputManager.moveUnit.status = Unit.Status.Idle;
		gameManager.inputManager.FreePanel();
	}

}
