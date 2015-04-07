using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	private Sprite[] mapSprite;
	public JSONObject mapJson;
	public GameObject prefab;
	
	public static int height;
	public static int width;
	
	// Use this for initialization
	void Awake () {
		mapSprite = Resources.LoadAll<Sprite>("terrain");
		TextAsset bindata= Resources.Load("map") as TextAsset;
		mapJson = new JSONObject(bindata.ToString());
		
		height = (int)mapJson.GetField("height").n;
		width = (int)mapJson.GetField("width").n;
		drawMap( mapJson.GetField("layers").list );
		
	}
	
	void drawMap(List<JSONObject> layers) {
		GameObject mapBox = new GameObject("Map");

		for (int order = 0; order < layers.Count; order++ ) {
			int i = 0;
			
			for (int y = height; y > 0; y-- ) {
				for (int x = 1; x <= width; x++ ) {
					GameObject mapMaster = Instantiate(prefab, new Vector3(x, y, -2), prefab.transform.rotation) as GameObject;
					mapMaster.transform.parent =  mapBox.transform;
					mapMaster.AddComponent<gridHighlight>();
					mapMaster.GetComponent<gridHighlight>().gridPosition = new Vector2(x, y);
					mapMaster.transform.localScale = new Vector2(0.95f, 0.95f);
					mapMaster.name = new Vector2(x, y).ToString();
					SpriteRenderer masterSprite = mapMaster.GetComponent<SpriteRenderer>();
					masterSprite.sortingOrder = 5;


					List<JSONObject> json = layers[order].GetField("data").list;
					int index =(int) json[i].n - 1;
						if (json[i].n != 0) {
							GameObject singlemap = Instantiate(prefab, new Vector2(x, y), prefab.transform.rotation) as GameObject;
							singlemap.transform.parent = mapMaster.transform;
							singlemap.GetComponent<SpriteRenderer>().sprite = mapSprite[ index ];
							singlemap.GetComponent<SpriteRenderer>().sortingOrder = order;
							singlemap.transform.localScale = new Vector2(3.15f, 3.15f);
						}
					masterSprite.sprite = Resources.Load<Sprite>("white");
					masterSprite.color = new Color(masterSprite.color.r, masterSprite.color.g, masterSprite.color.b, 0.1f );
					i++;
					}
				}	
		}
	}
	
}
