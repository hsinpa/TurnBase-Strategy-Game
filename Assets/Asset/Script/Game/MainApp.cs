using UnityEngine;
using System.Collections;
using ObserverPattenr;

public class MainApp : Singleton<MainApp> {
	protected MainApp () {} // guarantee this will be always a singleton only - can't use the constructor!

	public GameManager game { get { return transform.FindChild("game").GetComponent<GameManager>(); } }
	public GameUIManager ui { get { return transform.FindChild("ui").GetComponent<GameUIManager>(); } }

	public Subject subject;


	void Awake() {
		subject = new Subject();

		subject.addObserver(game);
		subject.addObserver(ui);

		game.SetUp(this);
		ui.SetUp(this);

		subject.notify( EventFlag.Game.EnterGame );
	}
}