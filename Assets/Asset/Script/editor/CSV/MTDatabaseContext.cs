using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

using System.Linq;
using Utility;
using System.Text.RegularExpressions;
/// <summary>
/// Organize gameobjects in the scene.
/// </summary>
public class MTDatabaseContext : Object
{
    const float LOAD_PART = 0.25f;

	const string PREFAB_FOLDER = "Assets/Asset/Prefab";
	const string UI_FOLDER = "Assets/project/asset/ui";
    /// <summary>
    /// Main app instance.
    /// </summary>
	static void UnityDownloadGoogleSheet(Dictionary<string, string> url_clone) {
		string path = "Assets/Resources/Database";

		if (url_clone.Count > 0) {
			KeyValuePair<string, string> firstItem = url_clone.First();

			WebRequest myRequest = WebRequest.Create (firstItem.Value); 

		      //store the response in myResponse 
		      WebResponse myResponse = myRequest.GetResponse(); 

		      //register I/O stream associated with myResponse
		      Stream myStream = myResponse.GetResponseStream ();

		      //create StreamReader that reads characters one at a time
		      StreamReader myReader = new StreamReader (myStream); 

		      string s = myReader.ReadToEnd ();
		      myReader.Close();//Close the reader and underlying stream



			File.WriteAllText(path + "/" + firstItem.Key + ".csv", s);
			url_clone.Remove(firstItem.Key);
			UnityDownloadGoogleSheet(url_clone);
			Debug.Log(firstItem.Key);

        } else {
            Debug.Log("Done");
        }
    }

    
	[MenuItem("Assets/SRPG/Database/Reset", false, 0)]
	static public void Reset() {
		PlayerPrefs.DeleteAll();
		Caching.CleanCache ();
	}



    [MenuItem("Assets/SRPG/Database/DownloadGoogleSheet", false, 0)]
    static public void OnDatabaseDownload() {
		string url = "https://docs.google.com/spreadsheets/d/1MpGI68D769Xt3Kd1KEEHAcqpk50UwYF42KLycCpTSx8/pub?gid=:id&single=true&output=csv";
		UnityDownloadGoogleSheet(new Dictionary<string, string> {
			{ "srpg - attribute", Regex.Replace( url, ":id", "0")},
			{ "srpg - level", Regex.Replace( url, ":id", "937201795")},
			{ "srpg - map", Regex.Replace( url, ":id", "2128930390")},
		} );
    }

    [MenuItem("Assets/SRPG/Database/GeneratePrefab", false, 0)]
    static public void GeneratePrefab() {
		CreateLevel();

    }

    static public void CreateLevel() {
		CSVFile csvFile = new CSVFile( Resources.Load<TextAsset>("Database/srpg - level").text );

		MapPrefab[] mapPrefabs = AssetDatabase.LoadAllAssetsAtPath(PREFAB_FOLDER+"/Map/").OfType<MapPrefab>().ToArray();
		LevelPrefab[] levelPrefabs = AssetDatabase.LoadAllAssetsAtPath(PREFAB_FOLDER+"/Level/").OfType<LevelPrefab>().ToArray();
		
		LevelListPrefab mLevelInventory = (LevelListPrefab)AssetDatabase.LoadAssetAtPath(PREFAB_FOLDER+"/Level/LevelList.asset", typeof(LevelListPrefab));

		//Delete every previous prefab
		foreach (LevelPrefab p_level in mLevelInventory.levelList) {
			DeletePrefab<MapPrefab>(p_level._mapList);
		}
		DeletePrefab<LevelPrefab>(mLevelInventory.levelList);
		mLevelInventory.levelList.Clear();


		for(int i = 0; i < csvFile.length; i++) {
			int level = csvFile.Get<int>(i, "Level");
			LevelPrefab c_prefab = ScriptableObjectUtility.CreateAsset<LevelPrefab>(PREFAB_FOLDER+"/Level/", "level-"+level);

			c_prefab._level = level;
			c_prefab._mapList = CreateMap( level );


			mLevelInventory.levelList.Add( c_prefab );
			
			EditorUtility.SetDirty(c_prefab);
		}
		EditorUtility.SetDirty(mLevelInventory);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
    }

	static public List<MapPrefab> CreateMap(int p_level) {
		List<MapPrefab> mapPrefabs= new List<MapPrefab>();
		CSVFile mapFile = new CSVFile( Resources.Load<TextAsset>("Database/srpg - map").text );

		for(int i = 0; i < mapFile.length; i++) {
			string levelID = mapFile.Get<string>(i, "ID");
			EventFlag.WinCondition winCondition = (EventFlag.WinCondition) mapFile.Get<int>(i, "Win Condition");
			string[] levelSplit = levelID.Split('.');

			if (levelSplit[0].Equals ("level"+p_level)) {
				MapPrefab prefab = ScriptableObjectUtility.CreateAsset<MapPrefab>(PREFAB_FOLDER+"/Map/", "mapPrefab-"+p_level+"-"+i);
				EditorUtility.SetDirty(prefab);

				prefab._winCondition = winCondition;
				prefab._id = levelID;

				mapPrefabs.Add(prefab);
			}
		}

		return mapPrefabs;
    }

	static public void DeletePrefab<T>(List<T> p_list) where T : ScriptableObject {
		foreach (T p in p_list) {
			string pathToDelete = AssetDatabase.GetAssetPath(p);      
			AssetDatabase.DeleteAsset(pathToDelete);
		}
	}
}
