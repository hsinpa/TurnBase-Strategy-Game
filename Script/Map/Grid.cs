using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	
	public Vector3 gridWorldSize;
	public float nodeRadius;
	public LayerMask unwalkableMask;
	public Node[,] grid;
	
	float nodeDiameter;
	int gridSizeX, gridSizeY;	
	
	void Awake() {
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt( gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt( gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}
	
	void CreateGrid() {
		grid = new Node[ gridSizeX, gridSizeY];
		Vector3 worldBottemLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;
		
		for (int y = 0; y < gridSizeY; y++ ) {
			for (int x = 0; x < gridSizeX; x++ ) {
				Vector3 worldPoint = worldBottemLeft + Vector3.right * (x * nodeDiameter + nodeRadius ) - Vector3.down * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius));
				grid[x,y] = new Node(walkable, worldPoint);
			}
		}
	}
	
	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		
		int x = Mathf.RoundToInt(( gridSizeX - 1 ) * percentX);
		int y = Mathf.RoundToInt(( gridSizeY - 1 ) * percentY);
		return grid[x, y];
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));
		if (grid != null) {
			foreach (Node n in grid) {
				
				Gizmos.color = (n.walkable) ?  new Color (1,0,0,0.3f) : Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.05f));
			}
		}
	}
}
