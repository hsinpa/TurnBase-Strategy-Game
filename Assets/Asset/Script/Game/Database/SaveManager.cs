using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class SaveManager : MonoBehaviour {

	public int slotIndex  { get { return PlayerPrefs.GetInt("Save Slot Index", 0); } }
	public int maxSlotNum = 3;

	public JSONObject saveSlotJSON;

	public void SetUp() {
		GetSaveRecord( slotIndex );
	}

	//Save slot 1, 2, 3, 4=temporary
	public void GetSaveRecord(int p_index) {
		string filePath = Application.persistentDataPath+"/saveslot-"+ p_index +".json";

		FileStream fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read );
		StreamReader slotReadAsset = new StreamReader(fs);
		string readAssetContext = slotReadAsset.ReadToEnd();

		saveSlotJSON = (readAssetContext.Equals("")) ? CreateEmptySaveSlot() : new JSONObject( readAssetContext );

		PlayerPrefs.SetInt("Save Slot Index", p_index );
		
		fs.Flush();
		fs.Close();
	}

	public void DeleteSaveRecord(int p_index) {
		WriteJSONToDisk(CreateEmptySaveSlot().ToString(), p_index);

		//If delete the curretn save slot, then better reload it
		if (p_index == slotIndex) GetSaveRecord( p_index );
	}

	// ==================================== Convert Save Record to JSON =================================

	// public JSONObject ConvertDecisionToJSON() {

	// }

	public JSONObject GenerateJSONReport() {
		JSONObject json = new JSONObject();
		json.SetField("Username", "PAUL");
		json.SetField("SavePoint", "{}");
		return json;
	}


	/* Game Slot JSON Layout
		{
			"Slot Index" : {
				"Username" : string
				"Attributes" : [{"key" ,"value"}],
				"DecisionMaked" : [].
			}

	} */
	public JSONObject CreateEmptySaveSlot() {
		JSONObject emptyJSON = new JSONObject();

		emptyJSON.SetField("Username", "");
		emptyJSON.SetField("Level", 1);

		emptyJSON.SetField("Character", new JSONObject("[]"));
		emptyJSON.SetField("Inventory", new JSONObject("[]"));

		emptyJSON.SetField("SavePoint", new JSONObject("{}"));

		return emptyJSON;
	}

	public void WriteJSONToDisk(string p_jsonRaw, int p_index) {
		string filePath = Application.persistentDataPath+"/saveslot-"+ p_index +".json";
		StreamWriter slotWriteAsset = System.IO.File.CreateText(filePath);

		slotWriteAsset.WriteLine(p_jsonRaw); 
		slotWriteAsset.Flush();
		slotWriteAsset.Close();
	}
}
