using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	private Sprite[] mapSprite;
	public JSONObject mapJson;
	public GameObject prefab;
	
	private int height;
	private int width;
	
	// Use this for initialization
	void Start () {
		mapSprite = Resources.LoadAll<Sprite>("terrain");
		TextAsset bindata= Resources.Load("map") as TextAsset;
		mapJson = new JSONObject(bindata.ToString());
		
		height = (int)mapJson.GetField("height").n;
		width = (int)mapJson.GetField("width").n;
		drawMap( mapJson.GetField("layers").list );
		
	}
	
	void drawMap(List<JSONObject> layers) {
		for (int order = 0; order < layers.Count; order++ ) {
			int i = 0;
			
			for (int y = height; y > 0; y-- ) {
				for (int x = 0; x < width; x++ ) {
					
					List<JSONObject> json = layers[order].GetField("data").list;
					int index =(int) json[i].n - 1;
					Debug.Log(index);
						if (json[i].n != 0) {
							GameObject singlemap = Instantiate(prefab, new Vector2(x, y), prefab.transform.rotation) as GameObject;
							singlemap.GetComponent<SpriteRenderer>().sprite = mapSprite[ index ];
							singlemap.GetComponent<SpriteRenderer>().sortingOrder = order;
							singlemap.transform.localScale = new Vector2(3.1f, 3.1f);
						}
					i++;
					}
				}	
		}
	}
	
}
