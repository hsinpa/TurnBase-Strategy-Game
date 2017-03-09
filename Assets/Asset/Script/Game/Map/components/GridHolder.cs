using UnityEngine;
using System.Collections;

public class GridHolder : MonoBehaviour {
	public enum Status {Idle, Move, Attack }

	public Status gridStatus {
		get {
			return mGridStatus;
		}
		set {
			switch(value) {
				case Status.Idle:
				changeHighLight( Resources.Load<Sprite>("white"), 0.1f, false);

				break;
				case Status.Attack:
				changeHighLight( Resources.Load<Sprite>("red"), 0.7f, false);
				break;
				case Status.Move:
				changeHighLight( Resources.Load<Sprite>("green"), 0.7f, true);
				break;
			}
			mGridStatus = value;
		}
	}
	public UnitPlacementComponent mPlacementPoint = null;

	private Status mGridStatus = Status.Idle;

    public Vector2 gridPosition;
    public Tile tile;
	public bool canMove;

	//For PathFinding
	public float costSoFar = 0;

	public Vector2 attackPos;

	//AI Setting
	[HideInInspector]
	public float landScore;

    public void changeHighLight(Sprite s, float alpha, bool state) {
		SpriteRenderer spriteRenderer = transform.Find("HighLight").GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = s;
		spriteRenderer.color =  new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha );
		canMove = state;
    }
}	
