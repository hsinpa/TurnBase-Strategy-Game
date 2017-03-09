using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
		Map myScript = (Map)target;
        if(GUILayout.Button("Build Dungeon")) {
			myScript.Load( Camera.main );
        }

    }
}
