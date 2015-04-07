using UnityEngine;
using System.Collections;

public class cameraCtrl : MonoBehaviour {
	public GameObject player;
	// Use this for initialization
	void Start () {
		player.transform.position = new Vector2(0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
	}
}
