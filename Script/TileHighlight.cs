using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileHighlight : MonoBehaviour {
	public List<Vector2> finalNodeList = new List<Vector2>();
	private float movePoint = 0;
	private gridHighlight[] gridSet;
	private Vector2 originTile;

	void Start() {
	}

	public List<Vector2> findHighlight(Vector2 original, int point) {
		List<Vector2> walkedPath = new List<Vector2>();
		movePoint = point;
		originTile = original;

		List<Vector2> nodes = findConnectNode(original, new List<Vector2>() );
		Debug.Log(nodes.Count);
		finalNodeList = nodes;
		highlightCtrl(nodes, false);
		return nodes;
	}
	
	public void highlightCtrl( List<Vector2> nodes, bool isClose ) {
		foreach (Vector2 n in nodes) {
			if (n.x > 0 ||  n.x <= Map.width || n.y > 0 || n.y <= Map.height) {
				gridHighlight masterHighlight = GameObject.Find("Map/"+n.ToString()).GetComponent<gridHighlight>();
				if (!isClose) {
					masterHighlight.changeHighLight( Resources.Load<Sprite>("green"), 0.7f, true);
				} else {
					masterHighlight.changeHighLight( Resources.Load<Sprite>("white"), 0.1f, false);
				}
			}
		}
	}
	
	private List<Vector2> findConnectNode(Vector2 node, List<Vector2> nodeStorage) {
		movePoint --;

		List<Vector2> tempNodeList = new List<Vector2>();
					tempNodeList.Add(new Vector2(node.x+1, node.y));
					tempNodeList.Add(new Vector2(node.x-1, node.y));
					tempNodeList.Add(new Vector2(node.x, node.y+1));
					tempNodeList.Add(new Vector2(node.x, node.y-1));
		List<Vector2> tempNodeList2 = tempNodeList;	
		
		for (int i = 0; i < tempNodeList.Count; i++) {
				Vector2 n = tempNodeList[i];
			
			if (!checkRequirement(n)) {
				tempNodeList2.Remove(n);
				}
			}
		nodeStorage.AddRange(tempNodeList2);

		if (movePoint > 0) {
			foreach (Vector2 k in tempNodeList2) {
				findConnectNode(k, nodeStorage);
			}
		}
		return nodeStorage;
	}
	
	private bool checkRequirement(Vector2 n) {
		if ((n.x <= 0 || n.x > Map.width || n.y <= 0 || n.y > Map.height) || finalNodeList.Contains(n) || n == originTile ) {
			return false;
		} else {			
			return true;
		}
	}
}
