using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class CharacterWindow : EditorWindow {
	private int mWidth = 800, mSpace = 30, mRowLimit = 4;
	private Vector2 boxSize = new Vector2(150, 200);
	public Vector2 scrollPosition = Vector2.zero;
	private string mBase_path = "Assets/Asset/Prefab/Unit/";
	private CharacterListPrefab mCharacterInventory;


	[MenuItem ("Window/Customize/Character")]
	static void Init () {
			
		// Get existing open window or if none, make a new one:
		CharacterWindow window = (CharacterWindow)EditorWindow.GetWindow (typeof (CharacterWindow), false, "Character");
		window.minSize = new Vector2( 40 ,30);
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

		scrollPosition = GUI.BeginScrollView(new Rect(mSpace, mSpace, mWidth, mWidth), scrollPosition, new Rect(0, 0, mWidth-20, maxHeight));
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

	private void CreateBoxInfo(int p_index, CharacterPrefab p_prefab) {
		int row = Mathf.CeilToInt( p_index / mRowLimit ),
			column = p_index - (row * mRowLimit),
			ySpace =  mSpace*(row + 1),
			xSpace =  mSpace*(column + 1);

		Vector2 position = new Vector2( (column * boxSize.x ) + (xSpace), (row * boxSize.y ) + (ySpace) );
		
		GUI.Box(new Rect( position.x, position.y, boxSize.x, boxSize.y), p_prefab.name + p_index);
		//p_prefab._name = EditorGUILayout.TextField ("Item Name", p_prefab._name as string);
		p_prefab._name =  EditorGUI.TextField(new Rect( position.x+5, position.y+20, boxSize.x-10, 20), p_prefab._name ); 
		//p_prefab._image =  EditorGUI.ObjectField( new Rect( position.x+5, position.y+20, boxSize.x-10, 20), ); 
		p_prefab._image =  EditorGUI.ObjectField (new Rect( position.x+5, position.y+40, boxSize.x-10, 20),
		 (Object)p_prefab._image, typeof (Sprite), false) as Sprite;

		p_prefab._strength =  EditorGUI.IntField(new Rect( position.x+5, position.y+60, boxSize.x-10, 20), p_prefab._strength ); 

		p_prefab._defense =  EditorGUI.IntField(new Rect( position.x+5, position.y+80, boxSize.x-10, 20), p_prefab._defense ); 

		p_prefab._speed =  EditorGUI.IntField(new Rect( position.x+5, position.y+100, boxSize.x-10, 20), p_prefab._speed ); 

		p_prefab._skill =  EditorGUI.IntField(new Rect( position.x+5, position.y+120, boxSize.x-10, 20), p_prefab._skill ); 

		p_prefab._footspeed =  EditorGUI.IntField(new Rect( position.x+5, position.y+140, boxSize.x-10, 20), p_prefab._footspeed ); 

		//p_prefab._image = EditorGUILayout.ObjectField ("Item Icon", p_prefab._image, typeof (Sprite), false) as Sprite;

	}

	private void CreateNewObject() {
		CharacterPrefab c_prefab = ScriptableObjectUtility.CreateAsset<CharacterPrefab>(mBase_path );
		mCharacterInventory.characterList.Add(c_prefab);
		AssetDatabase.SaveAssets();
	}
}
