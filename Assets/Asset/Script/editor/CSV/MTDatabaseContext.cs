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
		} );
    }


}
