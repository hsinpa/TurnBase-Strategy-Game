using UnityEngine;
using System.Collections;

public class BasicAction : MonoBehaviour {
	GameManager gameManager;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("Map").GetComponent<GameManager>();
	}
		
	public void nextTurn() {
		foreach (Unit unit in gameManager.unitSet ) {
			if (unit.tag == "Player")
				unit.status = 0;
		}
	}
}
