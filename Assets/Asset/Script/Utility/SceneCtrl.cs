using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour {
	public void Load(string sceneName) {
		if (!SceneManager.GetSceneByName(sceneName).isLoaded)
			SceneManager.LoadScene(sceneName);
	}
}
