using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Archer : Weapon {

	public Archer() {
		
	}
	#region WeaponType implementation
	
	public override List<Vector2> GetAttackPoint (Vector2 node)
	{
		List<Vector2> tempNodeList = new List<Vector2>();
		tempNodeList.Add(new Vector2(node.x+2, node.y));
		tempNodeList.Add(new Vector2(node.x-2, node.y));
		tempNodeList.Add(new Vector2(node.x, node.y+2));
		tempNodeList.Add(new Vector2(node.x, node.y-2));
		
		tempNodeList.Add(new Vector2(node.x+1, node.y-1));
		tempNodeList.Add(new Vector2(node.x+1, node.y+1));
		tempNodeList.Add(new Vector2(node.x-1, node.y-1));
		tempNodeList.Add(new Vector2(node.x-1, node.y+1));
		return tempNodeList;
	}
	
	#endregion
	

}
