 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class CharacterWindow : EditorWindow {
	private int mWidth = 800, mSpace = 30, mRowLimit = 4;
	private Vector2 boxSize = new Vector2(150, 240);
	public Vector2 scrollPosition = Vector2.zero;
	private string mBase_path = "Assets/Asset/Prefab/Unit/";
	private CharacterListPrefab mCharacterInventory;

	[MenuItem ("Window/Customize/Character")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		CharacterWindow window = (CharacterWindow)EditorWindow.GetWindow (typeof (CharacterWindow), false, "Character");
		//window.minSize = new Vector2( 0 ,30);

		window.Show();
	}

	void  OnEnable () {
		mCharacterInventory = AssetDatabase.LoadAssetAtPath (mBase_path+"CharacterList.asset", typeof(CharacterListPrefab)) as CharacterListPrefab;
    }


	void OnGUI () {

		GUILayout.BeginHorizontal ();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Character", GUILayout.ExpandWidth(false))) {
                CreateNewObject();
            }
        GUILayout.EndHorizontal ();

		//To get the maximun height of scroll window
		int itemCount = mCharacterInventory.characterList.Count, maxRow = (Mathf.CeilToInt( itemCount / mRowLimit ) );
		float maxHeight = ((maxRow + 2) * mSpace) + (boxSize.y * (maxRow + 2));

		scrollPosition = GUI.BeginScrollView(new Rect(mSpace, mSpace, position.width, position.height), scrollPosition, new Rect(0, 0, mWidth-20, maxHeight));
		for (int i = 0; i < itemCount; i++) {
			CreateBoxInfo(i, mCharacterInventory.characterList[i]);
		}
		GUI.EndScrollView();

		//Save ScriptableObject
		if (GUI.changed) {
            EditorUtility.SetDirty(mCharacterInventory);
			foreach (CharacterPrefab ch_prefab in mCharacterInventory.characterList) EditorUtility.SetDirty(ch_prefab);
        }
	}

	// ================================= GUI Method ==============================
	private void CreateBoxInfo(int p_index, CharacterPrefab p_prefab) {
		int row = Mathf.CeilToInt( p_index / mRowLimit ),
			column = p_index - (row * mRowLimit),
			ySpace =  mSpace*(row + 1),
			xSpace =  mSpace*(column + 1);

		Vector2 new_position = new Vector2( (column * boxSize.x ) + (xSpace), (row * boxSize.y ) + (ySpace) );
		
		GUI.Box(new Rect( new_position.x, new_position.y, boxSize.x, boxSize.y), p_prefab.name + p_index);

		//Name
		EditorGUI.LabelField(new Rect( new_position.x+5, new_position.y+20, 40, 20), "Name :");
		p_prefab._name =  EditorGUI.TextField(new Rect( new_position.x+60, new_position.y+20, 80, 20), p_prefab._name ); 

		//Image
		Sprite p_sprite = (p_prefab._image == null) ? AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd") : p_prefab._image;


		Rect tr = p_sprite.textureRect;
		Rect r = new Rect(tr.x / p_sprite.textureRect.width, tr.y / p_sprite.textureRect.height,
						 tr.width / p_sprite.textureRect.width, tr.height / p_sprite.textureRect.height );

	//		GUI.DrawTextureWithTexCoords(new Rect( new_position.x + 5, new_position.y+45, tr.width,tr.height), p_sprite.texture, r, true);
		GUI.DrawTexture(new Rect( new_position.x + 5, new_position.y+45, 60, 60),
						p_sprite.texture, ScaleMode.StretchToFill, true, 10.0F);

		p_prefab._image =  EditorGUI.ObjectField (new Rect( new_position.x + (boxSize.x/2), new_position.y+45, 40, 20),
		 (Object)p_prefab._image, typeof (Sprite), false) as Sprite;

		//Stats Group
		int baseHeight =(int) new_position.y+90, intervalSpace = 20, r_index = 1;

		r_index = CreateTextRow(ref p_prefab._strength, ref p_prefab._strength_growth_rate, intervalSpace, baseHeight, new_position, r_index, "Strength" );
		r_index = CreateTextRow(ref p_prefab._defense, ref p_prefab._defense_growth_rate, intervalSpace, baseHeight, new_position, r_index, "Defense" );
		r_index = CreateTextRow(ref p_prefab._speed, ref p_prefab._speed_growth_rate, intervalSpace, baseHeight, new_position, r_index, "Speed" );
		r_index = CreateTextRow(ref p_prefab._skill, ref p_prefab._skill_growth_rate, intervalSpace, baseHeight, new_position, r_index, "Skill" );
		r_index = CreateTextRow(ref p_prefab._footspeed, ref p_prefab._footspeed_growth_rate, intervalSpace, baseHeight, new_position, r_index, "Foot Speed" );
	}

	private int CreateTextRow(ref int value, ref float growth_rate, int interval, int baseHeight, Vector2 p_position, int p_index, string p_title) {
		//Debug.Log("Base height " + baseHeight);
		EditorGUI.LabelField(new Rect( p_position.x+5, baseHeight +( interval * p_index), 60, 20), p_title + " :");
		//Value
		value =  EditorGUI.IntField(new Rect(p_position.x+70, baseHeight +( interval * p_index), 35, 20), value );
		//Growth Rate
		growth_rate =  EditorGUI.FloatField(new Rect( p_position.x+110, baseHeight +( interval * p_index), 30, 20), growth_rate ); 
		return p_index += 1;
	}


	// ================================   Tab Method =============================
	private void CreateNewObject() {
		CharacterPrefab c_prefab = ScriptableObjectUtility.CreateAsset<CharacterPrefab>(mBase_path );
		mCharacterInventory.characterList.Add(c_prefab);
		AssetDatabase.SaveAssets();
	}
}
