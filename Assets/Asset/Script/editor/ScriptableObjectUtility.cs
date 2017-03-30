using UnityEngine;
using UnityEditor;
using System.IO;
 
public static class ScriptableObjectUtility
{
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static T CreateAsset<T> (string p_path, string name = "") where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
 
//		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
//		if (path == "") 
//		{
//			path = "Assets";
//		} 
//		else if (Path.GetExtension (path) != "") 
//		{
//			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
//		}
// 		Debug.Log(path);
		string fileName = (name == "") ? "New " + typeof(T).ToString() + ".asset" : name +".asset";
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (p_path + fileName);
 
		AssetDatabase.CreateAsset (asset, assetPathAndName);
 
		AssetDatabase.SaveAssets ();
        AssetDatabase.Refresh();
		//EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;

		return asset;
	}
}