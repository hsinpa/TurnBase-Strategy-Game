using UnityEngine;
using System.Collections;

public class gridHighlight : MonoBehaviour {
    public Vector2 gridPosition;
        
    public void changeHighLight(Sprite s) {
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = s;
		spriteRenderer.color =  new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.3f );
		
    }
}	
