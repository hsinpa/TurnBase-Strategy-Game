using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Player {

public class AIPattern {
		GameManager mGameManager;
		GridManager gridManager;

		public AIPattern(GameManager p_gameManager) {
			mGameManager = p_gameManager;
			gridManager = mGameManager.map.gridManager;
		}

		public GridHolder FindBestAttackRoute(Unit p_unit) {
			List<GridHolder> grids = gridManager.FindPossibleRoute(p_unit, mGameManager.player);
			if (grids.Count > 0) {
				return CalculateBestLandPoint(grids, mGameManager.player.allUnits);
			}
			return null;
		}

		public void FindBestAttackTarget() {
			//Calculate all unit's attack score, find the most weakest one


		}

		public GridHolder CalculateBestLandPoint(List<GridHolder> grids, List<Unit> enemyUnits ) {

			for (int i = 0; i < grids.Count; i++) {
				GridHolder grid = grids[i];
				grid.landScore = grid.tile.defenseBonus * 2;
				List<float> collectUnitScore = new List<float>();

				foreach (Unit unit in enemyUnits) {
					collectUnitScore.Add ( -Vector2.Distance(grid.gridPosition, unit.transform.position ) );
				}

				grid.landScore = collectUnitScore.Max();
			}

			return grids.OrderByDescending(x => x.landScore).First();
		}
	}
}