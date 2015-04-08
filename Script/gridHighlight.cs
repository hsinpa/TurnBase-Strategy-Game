using UnityEngine;
using System.Collections;

public class gridHighlight : MonoBehaviour {
    public Vector2 gridPosition;
	public bool canMove;
        
    public void changeHighLight(Sprite s, float alpha, bool state) {
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = s;
		spriteRenderer.color =  new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha );
		canMove = state;
    }
}	
