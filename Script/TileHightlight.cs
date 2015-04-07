using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileHightlight : MonoBehaviour {
	private int movePoint;
	private gridHighlight[] gridSet;
	private List<Vector2> finalNodeList = new List<Vector2>();
	private Vector2 originTile;
	
	void Start() {
		//gridSet = GameObject.Find("Map").GetComponentsInChildren<gridHighlight>();
		findHighlight(new Vector2(1,1), 2);		
	}
	
	public List<Vector2> findHighlight(Vector2 original, int point) {
		List<Vector2> walkedPath = new List<Vector2>();
		movePoint = point;
		originTile = original;
		findConnectNode(original);
		highlight();
		return null;
	} 	
	
	private void highlight() {
		foreach (Vector2 n in finalNodeList) {	
			if (n.x > 0 && n.x <= Map.width && n.y > 0 && n.y <= Map.height) {
							gridHighlight masterHighlight = GameObject.Find("Map/"+n.ToString()).GetComponent<gridHighlight>();
							masterHighlight.changeHighLight( Resources.Load<Sprite>("red"));
			}
		}
	}
	
	private List<Vector2> findConnectNode(Vector2 node) {
		movePoint--;
		
		List<Vector2> tempNodeList = new List<Vector2>();
					tempNodeList.Add(new Vector2(node.x+1, node.y));
					tempNodeList.Add(new Vector2(node.x-1, node.y));
					tempNodeList.Add(new Vector2(node.x, node.y+1));
					tempNodeList.Add(new Vector2(node.x, node.y-1));
		List<Vector2> tempNodeList2 = tempNodeList;	
		
		for (int i = 0; i < tempNodeList.Count; i++) {
				Vector2 n = tempNodeList[i];
			
			if (!checkRequirement(n)) {
				tempNodeList2.RemoveAt(i);
				}
			}
			
		finalNodeList.AddRange(tempNodeList2);
		if (movePoint > 0) {
			foreach (Vector2 k in tempNodeList2) {
				findConnectNode(k);
			}
		}
		return finalNodeList;
	}
	
	private bool checkRequirement(Vector2 n) {
		if (finalNodeList.Contains(n) || n == originTile) {			
			return false;
		} else {			
			return true;
		}
	}
}
