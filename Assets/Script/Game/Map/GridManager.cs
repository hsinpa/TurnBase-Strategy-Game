using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utility;

public class GridManager : MonoBehaviour {
	public List<GridHolder> availableGridList = new List<GridHolder>();
	public Vector2 originTile;

	private Map map { get { return GetComponent<Map>(); } }
	public int unitLayer = 1 << 8;
	public APath aPathFinding;

	public void Prepare() {
		aPathFinding =  new APath(map); 
	}

	public List<GridHolder> FindPossibleRoute(Unit p_unit, Player.User enemy) {
		originTile = p_unit.unitPos;

		List<GridHolder> nodes = findConnectNode(p_unit, enemy.allUnits, p_unit.currentWeapon );
		availableGridList = nodes;
		highlightCtrl(nodes, false);
		return nodes;
	}

	public void highlightCtrl( List<GridHolder> nodes, bool isClose ) {
		foreach (GridHolder n in nodes) {
			if (!isClose) {
				n.tag = "GroundMove";
				n.changeHighLight( Resources.Load<Sprite>("green"), 0.7f, true);
			} else {
				n.tag = "GroundIdle";
				n.changeHighLight( Resources.Load<Sprite>("white"), 0.1f, false);
			}
		}
	}

	public void DrawPathLine(List<Vector2> pathList) {
		LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
		if (lineRenderer == null) lineRenderer = gameObject.AddComponent<LineRenderer>();

		//lineRenderer.enabled = (pathList.Count > 0);

        lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.sortingLayerName = "Foreground";
		lineRenderer.SetColors(GeneralSetting.shallow_red, GeneralSetting.dark_red);
		lineRenderer.SetVertexCount(pathList.Count );
		for (int i = 0; i < pathList.Count; i++) {
			lineRenderer.SetPosition (i, pathList[i]);
		}

	}

	public bool isUnitRestrict(Vector2 point) {
		Collider2D collide = Physics2D.OverlapPoint(point, unitLayer);
		if (collide && collide.tag == "Enemy") {
			return true;
		}
		return false;
	}

	public void showAttackGrid(Unit unit) {
		List<Vector2> canAttackGrid = new List<Vector2>();

		unit.currentWeapon.GetAttackPoint(unit.transform.position).ForEach(delegate(Vector2 obj) {
			if (map.grids.FindAll(x => x.gridPosition == obj).Count > 0) canAttackGrid.Add(obj);
		});

		foreach (Vector2 k in canAttackGrid) {
			GridHolder gridHolder = map.FindTileByPos(k);
			gridHolder.changeHighLight( Resources.Load<Sprite>("red"), 0.7f, true);
		}
	}

	//Find Walkable Node
	private List<GridHolder> findConnectNode(Unit mUnits, List<Unit> allUnits, Weapon p_weapon) {
		List<GridHolder> closeNode  = new List<GridHolder>();
		List<Vector2> openNode  = new List<Vector2>();

		GridHolder startGrid = map.FindTileByPos(mUnits.unitPos);
		startGrid.costSoFar = 0;
		openNode.Add(mUnits.unitPos);

		int movePoint = mUnits.footSpeed;


		while (openNode.Count > 0) {
			Vector2 node = openNode.First();
			GridHolder currentGrid = map.FindTileByPos(node);

			List<Vector2> neighborNodes = GetNeighbour(node);

			for (int i = 0; i < neighborNodes.Count; i++) {
				GridHolder refilterN = map.FindTileByPos(neighborNodes[i]);
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
				GridHolder refilterN = map.FindTileByPos(tempNodeList[i]);

				if (!map.grids.Contains( refilterN ) || refilterN.tile.cost < 0 ||
					isUnitRestrict(refilterN.gridPosition) || refilterN.gridPosition == originTile) {
						 tempNodeList2.Remove(tempNodeList[i] );
				}
			}
		return tempNodeList2;
	}

}
