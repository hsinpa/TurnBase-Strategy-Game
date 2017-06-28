using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utility;
using PathSolution;

public class GridManager : MonoBehaviour {
	public List<GridHolder> availableGridList = new List<GridHolder>();

	private Map map { get { return GetComponent<Map>(); } }
	public APath aPathFinding;
	public Dijkstra dijkstra;

	public void Prepare() {
		aPathFinding =  new APath(map);
		dijkstra =  new Dijkstra(map); 
	}

	public List<GridHolder> FindPossibleRoute(Unit p_unit, Player.User enemy) {
		List<GridHolder> nodes =  dijkstra.findConnectNode(p_unit, enemy.allUnits );
		List<GridHolder> attackNode = dijkstra.FindNodeWithWeaponRange( nodes, p_unit );

		availableGridList = nodes;
		ShowAttackGrid(attackNode);
		return nodes;
	}

	//Set all tile to idle
	public void ResetGrid(List<GridHolder> nodes) {
		nodes.ForEach(delegate(GridHolder obj) {
			obj.gridStatus = GridHolder.Status.Idle;
			obj.attackPosList.Clear();
		});
	}

	public void ClearPathLine() {
		DrawPathLine(new List<Vector2>());
	}

	public void DrawPathLine(List<Vector2> pathList) {
		LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
		if (lineRenderer == null) lineRenderer = gameObject.AddComponent<LineRenderer>();
		
		//lineRenderer.enabled = (pathList.Count > 0);
		lineRenderer.startWidth = 0.2f;
		lineRenderer.endWidth = 0.2f;

		// lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.sortingLayerName = "Foreground";

		lineRenderer.startColor = GeneralSetting.shallow_yellow;
		lineRenderer.endColor = GeneralSetting.dark_yellow;

		lineRenderer.positionCount = pathList.Count;
		for (int i = 0; i < pathList.Count; i++) {
			lineRenderer.SetPosition (i, pathList[i]);
		}

	}

	//Use to find unit's attack range (Maximun range not included)
	public void FindAttackGrid(Unit unit) {
		List<GridHolder> canAttackGrid = new List<GridHolder>();

		unit.currentWeapon.GetAttackPoint(unit.unitPos).ForEach(delegate(Vector2 obj) {
			if (map.grids.FindAll(x => x.gridPosition == obj).Count > 0) {
				GridHolder grid = map.FindTileByPos(obj);
				grid.gridStatus = GridHolder.Status.Attack;
				canAttackGrid.Add( grid );
			}
		});
	}
	
	//Change Tile to red color
	public void ShowAttackGrid(List<GridHolder> p_grid ) {
		foreach (GridHolder k in p_grid) {
			k.gridStatus = GridHolder.Status.Attack;
		}
	}

	public Vector2 FindBestAttackPos( Vector2 default_pos, List<Vector2> p_attackPos, List<Tile> p_recordTile ) {
            Vector2 bestStandPoint = default_pos;
            //Pick the nearest attackpoint, unit stand before
            for (int i = p_attackPos.Count - 1; i >= 0; i--) {
                  if (p_recordTile.Count(x=>x.position == p_attackPos[i]) > 0 ) {
                        bestStandPoint = p_recordTile.Find(x=>x.position == p_attackPos[i]).position;
                        break;
                }
            }
		return bestStandPoint;
	}

}
