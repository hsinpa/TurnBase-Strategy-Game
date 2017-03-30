using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using Utility;

public class LevelMapView : MonoBehaviour {
	private int mNodeIndex = 0;

	public void SetUp() {
		CSVFile levelMapcsv = new CSVFile( Resources.Load<TextAsset>( "Database/srpg - level").text );
		GenerateLevelNode(levelMapcsv);
	}

	public void GenerateLevelNode(CSVFile p_levelMapcsv) {
	    GameObject nodePrefab = Resources.Load<GameObject>("Prefab/UI/Menu/node");

		UtilityMethod.ClearChildObject( transform );

		for(int i = 0; i < p_levelMapcsv.length; i++) {
			int mapNum = p_levelMapcsv.Get<int>( i , "Map number"),
				level = p_levelMapcsv.Get<int>( i , "Level"),
				index = i;
			
			GameObject nodeObject = UtilityMethod.CreateObjectToParent(transform, nodePrefab );
			Button nodeButton = nodeObject.GetComponent<Button>();
			nodeObject.transform.Find("field").GetComponent<Text>().text = (index+1).ToString();

			nodeButton.onClick.AddListener(delegate() {
				mNodeIndex = index;
				MainApp.Instance.stringTag.tagList.Add( level.ToString() );
				MainApp.Instance.sceneCtrl.Load("Game");
			});


		}
			

	}
}
