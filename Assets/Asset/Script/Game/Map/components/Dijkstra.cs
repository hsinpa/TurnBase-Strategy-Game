using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using Utility;

namespace PathSolution {
	public class Dijkstra {
		Map mMap;
		Vector2 originTile;

		public Dijkstra(Map p_map) {
			mMap = p_map;
		}

		//Find Walkable Node
		public List<GridHolder> findConnectNode(Unit mUnits, List<Unit> allUnits) {
			List<GridHolder> closeNode  = new List<GridHolder>();
			List<Vector2> openNode  = new List<Vector2>();

			GridHolder startGrid = mMap.FindTileByPos(mUnits.unitPos);
			startGrid.costSoFar = 0;
			originTile=mUnits.unitPos;
			openNode.Add(mUnits.unitPos);

			int movePoint = mUnits.footSpeed;


			while (openNode.Count > 0) {
				Vector2 node = openNode.First();
				GridHolder currentGrid = mMap.FindTileByPos(node);

				List<Vector2> neighborNodes = GetNeighbour(node);

				for (int i = 0; i < neighborNodes.Count; i++) {
					GridHolder refilterN = mMap.FindTileByPos(neighborNodes[i]);
					int enemyNum = allUnits.Count(x=>x.unitPos == neighborNodes[i]);
					float p_costSoFar = currentGrid.costSoFar + refilterN.tile.cost;

					if (closeNode.Contains(refilterN) || p_costSoFar > movePoint || enemyNum > 0)  continue;
					if (openNode.Contains(neighborNodes[i])) {
						
					} else {
						refilterN.costSoFar = p_costSoFar;
						openNode.Add(neighborNodes[i]);
					}
				}

				openNode.Remove(node);
				currentGrid.gridStatus = GridHolder.Status.Move;
				closeNode.Add(currentGrid);
			}
			return closeNode;
		}

		private List<Vector2> GetNeighbour(Vector2 dot) {
			List<Vector2> tempNodeList = new List<Vector2>();
				tempNodeList.Add(new Vector2(dot.x+1, dot.y));
				tempNodeList.Add(new Vector2(dot.x-1, dot.y));
				tempNodeList.Add(new Vector2(dot.x, dot.y+1));
				tempNodeList.Add(new Vector2(dot.x, dot.y-1));

			List<Vector2> tempNodeList2 = new List<Vector2>(tempNodeList);	

				//Check if the tempNode is valid and not exist in nodeStorage
				for (int i = 0; i < tempNodeList.Count; i++) {
					GridHolder refilterN = mMap.FindTileByPos(tempNodeList[i]);

					if (!mMap.grids.Contains( refilterN ) || refilterN.tile.cost < 0 ||
						isUnitRestrict(refilterN.gridPosition) || refilterN.gridPosition == originTile) {
							tempNodeList2.Remove(tempNodeList[i] );
					}
				}
			return tempNodeList2;
		}

	public List<GridHolder> FindNodeWithWeaponRange(List<GridHolder> p_originalGrid, Unit p_unit) {
		List<GridHolder> AttackGrid = new List<GridHolder>();
		int UT_layer = GeneralSetting.unitLayer;


		foreach (GridHolder grid in p_originalGrid) {
			List<Vector2> p_attackPoint = p_unit.currentWeapon.GetAttackPoint( grid.gridPosition );
			//Check the attack standing point won't overlap with others
			Collider2D collides = Physics2D.OverlapPoint(grid.gridPosition, UT_layer);

			foreach (Vector2 point in p_attackPoint ) {
				GridHolder refilterN = mMap.FindTileByPos(point);

				//Check findNode and attackNodeStorage won't repeat
				if (p_originalGrid.Count(x=> x.gridPosition == point) <= 0 && 
					// AttackGrid.Count(x=> x.gridPosition == point) <= 0 &&
					//Unit overlap
					(collides == null || collides.transform.position == p_unit.transform.position ) &&
					//Grid Valid
					mMap.grids.Contains( refilterN )) {

						refilterN.attackPosList.Add( grid.gridPosition );

						if (AttackGrid.Count(x=> x.gridPosition == point) <= 0) {
							refilterN.attackPos = grid.gridPosition;
							AttackGrid.Add( refilterN );
						}
							
				}

			}
		}

		return AttackGrid;
	}


		private bool isUnitRestrict(Vector2 point) {
			Collider2D collide = Physics2D.OverlapPoint(point, GeneralSetting.unitLayer);
			if (collide && collide.tag == "Enemy") {
				return true;
			}
			return false;
		}

	}
}