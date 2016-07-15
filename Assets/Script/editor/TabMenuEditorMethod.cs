using UnityEngine;
using System.Collections;
using UnityEditor;

public class TabMenuEditorMethod {

	[MenuItem("Assets/Create/CharacterList")]
    public static CharacterListPrefab  CreateCharacterList() {
		CharacterListPrefab asset = ScriptableObject.CreateInstance<CharacterListPrefab>();
		AssetDatabase.CreateAsset(asset, "Assets/Asset/Prefab/Unit/CharacterList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

}
