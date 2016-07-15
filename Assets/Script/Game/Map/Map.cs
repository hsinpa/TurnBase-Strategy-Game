using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Map : MonoBehaviour {

	private Sprite[] mapSprite;
	public JSONObject mapJson;
	public static int height;
	public static int width;
	public List<GridHolder> grids = new List<GridHolder>();

	public GridManager gridManager;


//	// Use this for initialization
//	void Awake () {
//		Load();
//	}

	public void Load() {
		mapSprite = Resources.LoadAll<Sprite>("tileSet");
		TextAsset bindata= Resources.Load("Database/Map/map02") as TextAsset;
		mapJson = new JSONObject(bindata.ToString());
		
		height = (int)mapJson.GetField("height").n;
		width = (int)mapJson.GetField("width").n;
		DrawMap( mapJson.GetField("layers").list );
		gridManager = GetComponent<GridManager>();
		gridManager.Prepare();
	}


	public void DrawMap(List<JSONObject> layers) {
		GameObject prefab = Resources.Load<GameObject>("Prefab/Map/EmptyBlock");
		GameObject gameBoard = new GameObject("GameBoard");
		for (int order = 0; order < layers.Count; order++ ) {
			int i = 0;
			
			for (int y = height; y > 0; y-- ) {
				for (int x = 1; x <= width; x++ ) {
					if (layers[order].GetField("type").str == "tilelayer") {
						DrawLayer(layers[order], gameBoard, prefab, new Vector2(x,y), i, order);
					}

					i++;
					}
			}	
		}
	}

	public void DrawLayer( JSONObject layer, GameObject gameBoard, GameObject prefab, Vector2 pos, int imageIndex, int orderIndex ) {
		JSONObject tileJSON = new JSONObject(Resources.Load<TextAsset>("Database/Tiles").text);
		string layerTitle = layer.GetField("name").str;
					GridHolder gridScript = FindTileByPos(pos);
					GameObject mapMaster;

					if (gridScript == null) {
						mapMaster = Instantiate(prefab, pos, prefab.transform.rotation) as GameObject;
						mapMaster.transform.parent =  gameBoard.transform;
						mapMaster.layer = 9;
						gridScript = mapMaster.AddComponent<GridHolder>();
						gridScript.gridPosition = pos;
						gridScript.tile = new Tile(gridScript.gridPosition, (int)pos.x, (int)pos.y, layer.GetField("properties"));

						grids.Add(gridScript);

						mapMaster.GetComponent<BoxCollider2D>().enabled = true;
						mapMaster.transform.localScale = new Vector2(0.95f, 0.95f);
						mapMaster.name = pos.ToString();
						SpriteRenderer masterSprite = mapMaster.GetComponent<SpriteRenderer>();
						masterSprite.sortingOrder = 5;
						masterSprite.sprite = Resources.Load<Sprite>("white");
						masterSprite.color = new Color(masterSprite.color.r, masterSprite.color.g, masterSprite.color.b, 0.02f );
					} else {
						mapMaster = gameBoard.transform.FindChild(pos.ToString()).gameObject;
					}

					List<JSONObject> json = layer.GetField("data").list;
					int index =(int) json[imageIndex].n - 1;
					if (json[imageIndex].n != 0) {
							gridScript.tile.UpdateInfo(layer.GetField("properties"));

							GameObject singlemap = Instantiate(prefab, pos, prefab.transform.rotation) as GameObject;
							singlemap.transform.parent = mapMaster.transform;
							singlemap.GetComponent<SpriteRenderer>().sprite = mapSprite[ index ];
							singlemap.GetComponent<SpriteRenderer>().sortingOrder = orderIndex;
							singlemap.transform.localScale = new Vector2(6.6f, 6.6f);
					}


	}


	public void DrawObject() {

	}


	public GridHolder FindTileByPos(Vector2 _pos) {
		return grids.Find(x => x.tile.position == _pos);
	}



}
