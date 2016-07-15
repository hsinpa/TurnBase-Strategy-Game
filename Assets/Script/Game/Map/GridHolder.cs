using UnityEngine;
using System.Collections;

public class GridHolder : MonoBehaviour {
    public Vector2 gridPosition;
    public Tile tile;
	public bool canMove;

	//For PathFinding
	public float costSoFar = 0;


	//AI Setting
	[HideInInspector]
	public float landScore;

    public void changeHighLight(Sprite s, float alpha, bool state) {
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = s;
		spriteRenderer.color =  new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha );
		canMove = state;
    }
}	
