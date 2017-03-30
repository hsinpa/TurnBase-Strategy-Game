using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player {
	public class AIManager : User {
		AIPattern mAIPattern;
		
		public IEnumerator Think() {
			CameraCtrl camera = Camera.main.GetComponent<CameraCtrl>();
				foreach (Unit unit in allUnits) {
					
					//If player has no minion anymore
					if (gm.player.allUnits.Count <= 0) break;

					//camera.StartFollowing(unit);
					GridHolder bestMoveToGrid = mAIPattern.FindBestAttackRoute(unit);

					//Move
					gm.inputManager.MoveUnitFromPath( unit, gm.inputManager.FindPath(unit.transform.position, bestMoveToGrid.gridPosition));

			        //React
					yield return StartCoroutine(PerformAction( unit ));

					//camera.StopFollowing();
					gm.map.gridManager.ResetGrid(gm.map.grids);
				}
			gm.EndTurn();
		}



		IEnumerator PerformAction(Unit p_unit) {
			yield return new WaitForSeconds(0.5f);

			//Attack
			Unit p_target = mAIPattern.FindBestAttackTarget(p_unit, gm.player.allUnits);
			yield return new WaitForSeconds(0.2f);

			
		}




		//====================================   Geneneral Config   ====================================
		public override void PreLoad (GameManager p_gManager, EventFlag.UserType p_uType) {
			base.PreLoad (p_gManager, p_uType);
			mAIPattern = new AIPattern(p_gManager);
		}

		public override void StartTurn () {
			Debug.Log("Enemy's Turn");
			StartCoroutine(Think());
		}

		public override void EndTurn () {
			gm.gameState.VerifyGameState();
		}
	}
}