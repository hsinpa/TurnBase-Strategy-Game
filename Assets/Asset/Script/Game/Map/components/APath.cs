using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathSolution {
	public class APath {
		Map mMap;

		public APath(Map p_map) {
			mMap = p_map;
		}
			
		public List<Tile> FindPath(GridHolder startGrid, GridHolder targetGrid) {

			List<Tile> openSet = new List<Tile>();
			HashSet<Tile> closeSet = new HashSet<Tile>();
			openSet.Add(startGrid.tile);
			
			while (openSet.Count > 0) {
				Tile currentNode = openSet[0];
				for (int i = 1; i < openSet.Count; i++) {
					if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost <currentNode.hCost) {
						currentNode = openSet[i];
					}
				}
				
				openSet.Remove(currentNode);
				closeSet.Add (currentNode);
				if (currentNode == targetGrid.tile) {
					return RetracePath(startGrid.tile, targetGrid.tile);
				}
				
				foreach (Tile neighbour in GetNeighbours(currentNode)) {

					if (!neighbour.walkable || closeSet.Contains(neighbour)) {
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetGrid.tile);
						neighbour.parent = currentNode;
						
						if (!openSet.Contains(neighbour)) {
							openSet.Add(neighbour);
						}
					}
					
				}
			}

			return new List<Tile>();	
		}
		
		List<Tile> RetracePath(Tile startNode, Tile endNode) {
			List<Tile> path = new List<Tile>();
			Tile currentNode = endNode;
			
			while (currentNode != startNode) {
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			path.Reverse();
			return path;
		}
		
		int GetDistance(Tile nodeA, Tile nodeB) {
			int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
			int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
			
			if (dstX > dstY) {
				return 14*dstY + 10 * (dstX - dstY);
			} else {
				return 14*dstX + 10 * (dstY - dstX);
			}
			
			
		}

		public List<Tile> GetNeighbours(Tile node) {
			List<Tile> neighbours = new List<Tile>();
			Vector2[] directionSet = new Vector2[] {new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0)  };
			foreach (Vector2 dirSet in directionSet) {
					int checkX = node.gridX + (int)dirSet.x;
					int checkY = node.gridY + (int)dirSet.y;

					GridHolder grid = mMap.FindTileByPos(new Vector2(checkX, checkY ) );

				if (checkX > 0 && checkX <= Map.width && checkY > 0 && checkY <= Map.height && grid != null &&
						mMap.gridManager.availableGridList.Contains(grid)) {
						neighbours.Add( grid.tile );
					}
			}
			return neighbours;
		}
	}
}