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
				//Not Move yet, find if there is someone to hit aside
				GridHolder attackGrids = FindBestAttackTarget(gridManager.dijkstra.FindNodeWithWeaponRange( grids, p_unit ), mGameManager.player.allUnits);
				if (attackGrids != null) return attackGrids;

				//No target beside, then try to find a best target from walking to them
				return CalculateBestLandPoint(grids, mGameManager.player.allUnits);
			}

			return null;
		}


		public Unit FindBestAttackTarget(Unit p_unit, List<Unit> enemyUnits) {
			List<Unit> possibleTarget= new List<Unit>();

			foreach (Vector2 point in p_unit.currentWeapon.GetAttackPoint(p_unit.unitPos)) {
				possibleTarget.AddRange(enemyUnits.FindAll(x=> x.unitPos == point));
			}

			if (possibleTarget.Count > 0) {
				//Hit the one with less hp
				Unit target = possibleTarget.OrderBy(x => x.hp).First();
				p_unit.Attack(target, mGameManager.map.FindTileByPos(target.unitPos ));
				return target;
			}

			return null;
		}

		//Calculate all unit's attack score, find the most weakest one
		public GridHolder FindBestAttackTarget(List<GridHolder> grids, List<Unit> enemyUnits) {
			List<GridHolder> possibleGrid = new List<GridHolder>();

			for (int i = 0; i < grids.Count; i++) {
				GridHolder grid = grids[i];
				//reset landscore
				// grid.landScore=0;
				foreach (Unit unit in enemyUnits) {
					 if (grid.gridPosition == unit.unitPos) {
						grid.landScore = unit.hp;
						possibleGrid.Add(grid);
					 }
				}
			}


			if (possibleGrid.Count > 0) {
				//Return the best value target
				return mGameManager.map.FindTileByPos(possibleGrid.OrderBy(x => x.landScore).First().attackPos);
			}

			return null;
		}


		public GridHolder CalculateBestLandPoint(List<GridHolder> grids, List<Unit> enemyUnits ) {

			for (int i = 0; i < grids.Count; i++) {
				GridHolder grid = grids[i];
				grid.landScore = grid.tile.defenseBonus * 2;
				List<float> collectUnitScore = new List<float>();

				//Give score by the distance between enemy 
				foreach (Unit unit in enemyUnits) {
					collectUnitScore.Add ( -Vector2.Distance(grid.gridPosition, unit.transform.position ) );
				}

				grid.landScore = collectUnitScore.Max();
			}

			return grids.OrderByDescending(x => x.landScore).First();
		}
	}
}