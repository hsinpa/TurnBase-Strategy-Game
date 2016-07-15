using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player {
	public class AIManager : User {
		AIPattern mAIPattern;
		
		public IEnumerator Think() {
			CameraCtrl camera = Camera.main.GetComponent<CameraCtrl>();

			foreach (Unit unit in allUnits) {
				GridHolder bestMoveToGrid = mAIPattern.FindBestAttackRoute(unit);


				camera.StartFollowing(unit);
				gm.inputManager.MoveUnit( unit, gm.inputManager.FindPath(unit.transform.position, bestMoveToGrid.gridPosition));
						
		        yield return new WaitForSeconds(1);
				camera.StopFollowing();
				gm.map.gridManager.highlightCtrl(gm.map.grids, true);
			}

			gm.EndTurn();
		}



		public void PerformAction() {
			
		}




		//====================================   Geneneral Config   ====================================
		public override void PreLoad (GameManager p_gManager) {
			base.PreLoad (p_gManager);
			mAIPattern = new AIPattern(p_gManager);
		}

		public override void StartTurn () {
			Debug.Log("Enemy's Turn");
			StartCoroutine(Think());
		}

		public override void EndTurn () {
			
		}
	}
}