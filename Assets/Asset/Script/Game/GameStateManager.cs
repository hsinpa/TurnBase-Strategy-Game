using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
	GameManager gameManager { get { return transform.GetComponent<GameManager>(); } }


	// ============================ Verify Win Lose ==================================
	//check win , lose or nothing happen
	public void VerifyGameState() {
		//Check Win first
		if (VerifyWinCondition()) {
			MainApp.Instance.subject.notify( EventFlag.Game.NextMap );
		}

		//No win, then check lose
		if (VerifyLoseCondition()) {
			MainApp.Instance.subject.notify( EventFlag.Game.GameEnd );
		}
	}

	public bool VerifyWinCondition() {
		bool isWin = false;

		isWin = OutOfUnit( gameManager.enemy.allUnits );

		return isWin;
	}

	public bool VerifyLoseCondition() {
		bool isLose = false;

		isLose = OutOfUnit( gameManager.player.allUnits );

		return isLose;
	}
	
	// ============================ WinLoseComponent ==================================
	//No more units in players
	public bool OutOfUnit( List<Unit> p_units) {
		return ( p_units.Count <= 0 );
	}

	// ============================ Others ==================================

	//Check special event in the tile character is landing
	public void VerifyTile(GridHolder p_grid, MapPrefab p_map) {
		foreach (UnitPlacementComponent p_component in p_grid.mPlacementPoint) {
			switch( p_component.placementType) {
				case EventFlag.PlacementType.TakeOff :
					if (p_map._winCondition == EventFlag.WinCondition.KillAll) {
						//Check if no enemy exist, if not, to next map
						if (MainApp.Instance.game.enemy.allUnits.Count == 0) {
							Debug.Log("Kill All : Next Map ");
						}
							MainApp.Instance.subject.notify( EventFlag.Game.NextMap );

							Debug.Log("Kill All : Enemy Exist ");
					} else if ( p_map._winCondition == EventFlag.WinCondition.Occupy ) {
						//To next map
						Debug.Log("Occupy");
						//MainApp.Instance.subject.notify( EventFlag.Game.NextMap );
					}
				break;

				case EventFlag.PlacementType.Evacuation :

				break;
			}
		}
	}

}
