using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{

    int mLevel, mMap;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
		Map myScript = (Map)target;

    	mLevel = EditorGUILayout.IntField("Level :", 1);
    	mMap = EditorGUILayout.IntField("Map :", 1);

        if(GUILayout.Button("Build Dungeon")) {
			myScript.SetUp( );
        	myScript.LoadMap( "level"+mLevel+"."+mMap );
        }

    }
}
