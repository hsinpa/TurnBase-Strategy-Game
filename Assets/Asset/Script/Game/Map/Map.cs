using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using Utility;

public class Map : MonoBehaviour {

	private Sprite[] mapSprite;
	public JSONObject mapJson;
	public static int height;
	public static int width;

	public List<GridHolder> grids = new List<GridHolder>();
	public List<UnitPlacementComponent> placements = new List<UnitPlacementComponent>();

	private CSVFile attributeCSV;
	public GridManager gridManager;

	public void SetUp() {
		mapSprite = Resources.LoadAll<Sprite>("tileSet");
		attributeCSV = new CSVFile( Resources.Load<TextAsset>( "Database/srpg - attribute" ).text );

		gridManager = GetComponent<GridManager>();
		gridManager.Prepare();

	}

	public void LoadMap(string p_mapname ) {
		TextAsset bindata= Resources.Load("Database/Map/"+p_mapname) as TextAsset;
		mapJson = new JSONObject(bindata.ToString());
		height = (int)mapJson.GetField("height").n;
		width = (int)mapJson.GetField("width").n;

		DrawMap( mapJson.GetField("layers").list );
	}

	public GameObject PrepareHolderObject() {
		if ( transform.FindChild("tiles") != null ) {
			if (Application.isEditor) {
				GameObject.DestroyImmediate( transform.FindChild("tiles").gameObject );			
			} else {
				GameObject.Destroy( transform.FindChild("tiles").gameObject );			
			}
		}
		GameObject gameBoard = new GameObject("tiles");
		gameBoard.transform.SetParent(transform);

		return gameBoard;
	}

	public void DrawMap(List<JSONObject> layers) {
		GameObject prefab = Resources.Load<GameObject>("Prefab/Map/EmptyBlock");
		GameObject gameBoard = PrepareHolderObject();
		grids.Clear();

		for (int order = 0; order < layers.Count; order++ ) {
			int i = 0;
			
			for (int y = height; y > 0; y-- ) {
				for (int x = 1; x <= width; x++ ) {
					string tileType = layers[order].GetField("type").str;
					switch (tileType) {
						case "tilelayer":
							DrawLayer(layers[order], gameBoard, prefab, new Vector2(x,y), i, order);
						break;
					}

					i++;
				}
			}	

			if (layers[order].GetField("type").str == "objectgroup") {
							DrawPlacement( layers[order].GetField("objects").list );
			}
		}
	}

	//Draw placement from Tiles's object json
	public void DrawPlacement(List<JSONObject> objects  ) {
		placements.Clear();

		for (int i = 0; i < objects.Count; i++ ) {
			JSONObject json = objects[i];

			int x = Mathf.RoundToInt( json.GetField("x").n / json.GetField("width").n ),
				y = Mathf.RoundToInt( json.GetField("y").n / json.GetField("height").n );
			Vector2 position = new Vector2(x, y);
			position = ConvertObjectPos( position );

			EventFlag.UserType userType = UtilityMethod.ParseEnum<EventFlag.UserType>( json.GetField("name").str );
			EventFlag.PlacementType placementType = UtilityMethod.ParseEnum<EventFlag.PlacementType>( json.GetField("type").str );

		UnitPlacementComponent placementPoint = new UnitPlacementComponent( userType, placementType, position, json.GetField("properties") ); 
		GridHolder gridScript = FindTileByPos(position);

		if (gridScript != null) {
				gridScript.mPlacementPoint.Add(placementPoint);
				placements.Add( placementPoint );
			}
		}
	}

	public void DrawLayer( JSONObject layer, GameObject gameBoard, GameObject prefab, Vector2 pos, int imageIndex, int orderIndex ) {
		//string layerTitle = layer.GetField("name").str;
		GridHolder gridScript = FindTileByPos(pos);
		GameObject mapMaster;

		if (gridScript == null) {
			mapMaster = Instantiate(prefab, pos, prefab.transform.rotation) as GameObject;
			mapMaster.transform.parent =  gameBoard.transform;
			mapMaster.layer = 9;
			gridScript = mapMaster.AddComponent<GridHolder>();
			gridScript.gridPosition = pos;
			gridScript.tile = new Tile(gridScript.gridPosition, (int)pos.x, (int)pos.y,
										GetPropertiesByCSV( layer.GetField("name").str));

			grids.Add(gridScript);

			mapMaster.GetComponent<BoxCollider2D>().enabled = true;
			mapMaster.name = pos.ToString();
			mapMaster.GetComponent<SpriteRenderer>().enabled = false;

			//Generate hightlighObject
			SpriteRenderer highlightSprite = DrawObject(mapMaster.transform, prefab, Resources.Load<Sprite>("white"), 
														new Vector2(0.95f, 0.95f), "HighLight", 5);
			highlightSprite.color = new Color(highlightSprite.color.r, highlightSprite.color.g, highlightSprite.color.b, 0.02f );

			// masterSprite.sprite = Resources.Load<Sprite>("white");
		} else {
			mapMaster = gameBoard.transform.FindChild(pos.ToString()).gameObject;
		}

		List<JSONObject> json = layer.GetField("data").list;
		int index =(int) json[imageIndex].n - 1;
		if (json[imageIndex].n != 0) {
			gridScript.tile.UpdateInfo(	GetPropertiesByCSV( layer.GetField("name").str) );

			SpriteRenderer mapSpriteObject = DrawObject(mapMaster.transform, prefab, mapSprite[ index ], Vector2.one, "LandForm",orderIndex);

			Bounds bounds = mapSpriteObject.GetComponent<SpriteRenderer>().sprite.bounds;

			Vector3 scaleVector = UtilityMethod.ScaleToWorldSize( bounds.size, 1 );
			mapSpriteObject.transform.localScale = scaleVector;
		}
	}


	public Unit GenerateUnitToPos(User p_user, GameObject prefab, UnitPlacementComponent p_placement) {
		string unitClass = p_placement.propertyJSON.GetField("class").str;

		Sprite sprite = UtilityMethod.LoadSpriteFromMulti( Resources.LoadAll<Sprite>("characters"), 
							p_placement.userType.ToString("g").ToLower() + "_"+ unitClass);
		
		Transform unitHolder = transform.FindChild("units");
		GameObject item = GameObject.Instantiate(prefab);
		Unit unit = item.GetComponent<Unit>();

		item.transform.SetParent( unitHolder );
		item.tag = p_placement.userType.ToString("g");
		unit.unitPos = p_placement.position;
		
		SpriteRenderer unitRenderer = item.GetComponent<SpriteRenderer>();
		unitRenderer.sprite = sprite;
		//If this sprite is not player, then flip its x vector
		if (p_placement.userType != EventFlag.UserType.Player) unitRenderer.flipX = true;

		Bounds bounds = unitRenderer.GetComponent<SpriteRenderer>().sprite.bounds;

		Vector3 scaleVector = UtilityMethod.ScaleToWorldSize( bounds.size, 1 );
		unitRenderer.transform.localScale = scaleVector;

		unit.Set(p_user, unitClass);
		p_user.allUnits.Add( unit );
		return unit;
	}

	
	public SpriteRenderer DrawObject(Transform p_holder, GameObject p_prefab, Sprite p_image, Vector2 p_size, string p_name, int p_order = 0 ) {
			GameObject prefabObject = Instantiate(p_prefab, new Vector3(0,0,0), p_prefab.transform.rotation) as GameObject;

			prefabObject.transform.SetParent(p_holder);
			prefabObject.transform.localScale =p_size;
			prefabObject.transform.localPosition = new Vector2(0, 0);
			prefabObject.name = p_name;

			SpriteRenderer hightlightSprite = prefabObject.GetComponent<SpriteRenderer>();
			hightlightSprite.sprite = p_image;
			hightlightSprite.sortingOrder = p_order;
			return hightlightSprite;
	}

	public void ClearUnits() {
		Transform unitHolder = transform.FindChild("units");
		UtilityMethod.ClearChildObject( unitHolder );
	}

	public JSONObject GetPropertiesByCSV(string p_landform) {
		JSONObject json = new JSONObject("{}");
		for (int i =0; i < attributeCSV.length; i++) {
			string rawAttrPair = attributeCSV.Get<string>(i, p_landform, "");
			if (rawAttrPair == "") continue;
			
			string[] attrPair = rawAttrPair.Split(':');
			json.SetField(attrPair[0], int.Parse(attrPair[1]));
		}
		return json;
	}

	public GridHolder FindTileByPos(Vector2 _pos) {
		return grids.Find(x => x.tile.position == _pos);
	}

	//Convert the upside down position of Objects to normal position
	public Vector2 ConvertObjectPos(Vector2 p_position) {
		return new Vector2( p_position.x + 1, height - p_position.y );
	}

}