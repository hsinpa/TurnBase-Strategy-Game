using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Player;
using ObserverPattenr;


public class MenuManager : Observer {
	LevelMapView levelMapView { get { return transform.Find("map").GetComponent<LevelMapView>(); } }

	public override void OnNotify (string p_event, params object[] p_objects) {
		switch(p_event) {
			case EventFlag.MenuUI.SetUp :
				Camera.main.GetComponent<CameraTransition>().ResetStart();
				levelMapView.SetUp();

			break;
		}
	}

	public void ToGame() {
		MainApp.Instance.sceneCtrl.Load("Game");
		
	}

}
