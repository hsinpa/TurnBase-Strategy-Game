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
			{ "srpg - character", Regex.Replace( url, ":id", "1905783139")},
		} );
    }


	static ScriptableListPrefab mLevelInventory;
    [MenuItem("Assets/SRPG/Database/GeneratePrefab", false, 0)]
    static public void GeneratePrefab() {
		mLevelInventory = (ScriptableListPrefab)AssetDatabase.LoadAssetAtPath(PREFAB_FOLDER+"/PrefabList.asset", typeof(ScriptableListPrefab));

        FileUtil.DeleteFileOrDirectory(PREFAB_FOLDER+"/Objects");
        AssetDatabase.CreateFolder(PREFAB_FOLDER, "Objects");
		mLevelInventory.prefabList.Clear();

		CreateMap();
		CreateLevel();
		CreateCharacter();

		EditorUtility.SetDirty(mLevelInventory);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
    }

    static public void CreateLevel() {
		CSVFile csvFile = new CSVFile( Resources.Load<TextAsset>("Database/srpg - level").text );		

		for(int i = 0; i < csvFile.length; i++) {
			int level = csvFile.Get<int>(i, "Level"),
				mapNum = csvFile.Get<int>(i, "Map number"),
				levelAverage = csvFile.Get<int>(i, "Average Level"),
				levelRange = csvFile.Get<int>(i, "Level Range");
			string id = csvFile.Get<string>(i, "ID");
			LevelPrefab c_prefab = ScriptableObjectUtility.CreateAsset<LevelPrefab>(PREFAB_FOLDER+"/Objects/", "level-"+level);
			EditorUtility.SetDirty(c_prefab);

			c_prefab._level = level;
			c_prefab._id = id;
			c_prefab._average_enemy_level = levelAverage;
			c_prefab._level_range = levelRange;


			for (int m = 1; m <= mapNum; m++) {
				Debug.Log(level+"."+m);

				MapPrefab mapPrefab  = (MapPrefab) mLevelInventory.prefabList.Find( x=>x._id == id+"."+m );
				Debug.Log(mapPrefab.name);
				c_prefab._mapList.Add(mapPrefab );
				
			}

			mLevelInventory.prefabList.Add( c_prefab );
			
		}
    }

	static public void CreateMap() {
		CSVFile mapFile = new CSVFile( Resources.Load<TextAsset>("Database/srpg - map").text );

		for(int i = 0; i < mapFile.length; i++) {
			string id = mapFile.Get<string>(i, "ID");
			EventFlag.WinCondition winCondition = (EventFlag.WinCondition) mapFile.Get<int>(i, "Win Condition");

			MapPrefab prefab = ScriptableObjectUtility.CreateAsset<MapPrefab>(PREFAB_FOLDER+"/Objects/", "mapPrefab-"+id);
			EditorUtility.SetDirty(prefab);

			prefab._winCondition = winCondition;
			prefab._id = id;

			mLevelInventory.prefabList.Add(prefab);
		}
    }

	static public void CreateCharacter() {
		CSVFile csvFile = new CSVFile( Resources.Load<TextAsset>("Database/srpg - character").text );

		for(int i = 0; i < csvFile.length; i++) {
			string id = csvFile.Get<string>(i, "ID");
			if (id == "") continue;

			CharacterPrefab prefab = ScriptableObjectUtility.CreateAsset<CharacterPrefab>(PREFAB_FOLDER+"/Objects/", "character-"+id);
			EditorUtility.SetDirty(prefab);

			string[] strength = csvFile.Get<string>(i, "Strength").Split('&'),
					defense = csvFile.Get<string>(i, "Defense").Split('&'),
					speed = csvFile.Get<string>(i, "Speed").Split('&'),
					skill = csvFile.Get<string>(i, "Skill").Split('&'),
					footSpeed = csvFile.Get<string>(i, "Foot Speed").Split('&'),
					hp = csvFile.Get<string>(i, "HP").Split('&');

			
			prefab._id = id;
			prefab._name = csvFile.Get<string>(i, "Name");
			prefab._class = csvFile.Get<string>(i, "Class");
			prefab._strength = int.Parse( strength[0] );
			prefab._defense = int.Parse(defense[0]);
			prefab._speed =  int.Parse( speed[0] );
			prefab._skill = int.Parse( skill[0] );
			prefab._footspeed = int.Parse( footSpeed[0] );
			prefab._hp = int.Parse( hp[0] );

			prefab._character_growth_rate = csvFile.Get<float>(i, "Growth Rate");
			prefab._strength_growth_rate = float.Parse( strength[1] );
			prefab._defense_growth_rate = float.Parse( defense[1] );
			prefab._speed_growth_rate = float.Parse( speed[1] );
			prefab._skill_growth_rate = float.Parse( skill[1] );
			prefab._footspeed_growth_rate = float.Parse( footSpeed[1] );
			prefab._hp_growth_rate = float.Parse( hp[1] );

			mLevelInventory.prefabList.Add(prefab);
		}
    }



	static public void DeletePrefab<T>(List<T> p_list) where T : ScriptablePrefab {
		foreach (T p in p_list) {
			string pathToDelete = AssetDatabase.GetAssetPath(p);      
			AssetDatabase.DeleteAsset(pathToDelete);
		}
	}
}
