using UnityEngine;
using System.Collections;
using ObserverPattenr;

public class MainApp : Singleton<MainApp> {
	protected MainApp () {} // guarantee this will be always a singleton only - can't use the constructor!
	public StringTag stringTag;

	public GameManager game { get { return transform.FindChild("game").GetComponent<GameManager>(); } }
	public GameUIManager ui { get { return transform.FindChild("ui").GetComponent<GameUIManager>(); } }
	public MenuManager menu { get { return transform.FindChild("ui").GetComponent<MenuManager>(); } }

	public SceneCtrl sceneCtrl { get { return transform.FindChild("utility").GetComponent<SceneCtrl>(); } }

	public Subject subject;


	void Awake() {
		stringTag = CreateSceneInfo();

		subject = new Subject();

		subject.addObserver(game);
		subject.addObserver(ui);
		subject.addObserver(menu);

		subject.notify( EventFlag.Game.SetUp );
		subject.notify( EventFlag.GameUI.SetUp );
		subject.notify( EventFlag.MenuUI.SetUp );

		subject.notify( EventFlag.Game.EnterGame );

		stringTag.type = "Hello World";
	}


	StringTag CreateSceneInfo() {
		GameObject sceneInfoObject = GameObject.Find("SceneInfo");
		if (sceneInfoObject == null) {
			sceneInfoObject = new GameObject("SceneInfo");
			sceneInfoObject.AddComponent<StringTag>();
		}

		StringTag tag = sceneInfoObject.GetComponent<StringTag>();


		DontDestroyOnLoad(sceneInfoObject);
		return tag;
	}


}