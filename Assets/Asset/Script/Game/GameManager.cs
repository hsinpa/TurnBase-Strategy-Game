using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Player;
using ObserverPattenr;

public class GameManager : Observer {
	public int round = 1;
	public List<User> users = new List<User>();
	public Map map;
	public User player {get { return users[0]; } }
	public User enemy {get { return users[1]; } }

	//User on its turn
	public User currentUser;
	public GameUIManager uiManager;
	public InputManager inputManager;
	public GameUtility gameUtility;

	public override void OnNotify (string p_event, params object[] p_objects) {
		switch(p_event) {
			case EventFlag.Game.EnterGame :
				OnNotify(EventFlag.Game.GameStart);

			break;


			case EventFlag.Game.GameStart :
				GameStart();
			break;

			case EventFlag.Game.RoundStart :
				RoundStart();
			break;

			case EventFlag.Game.RoundEnd :
				RoundEnd();
			break;

			case EventFlag.Game.GameEnd :
				GameOver();
			break;
		}
	}

	//Prepare anything prequisted ready
	public void SetUp(MainApp p_main) {
		map = transform.Find("map").GetComponent<Map>(); 
		map.Load( Camera.main );

		inputManager = transform.FindChild("user").GetComponent<InputManager>();
		inputManager.SetUp(this, p_main.ui);
		
		gameUtility = new GameUtility( this );
		uiManager = p_main.ui;

		//Enable UI Canvas
		uiManager.GetComponent<Canvas>().enabled = true;
	}

	public void AssignPlayer() {
		users.Add( gameUtility.CreateNewUser(EventFlag.UserType.Player, gameUtility.CreateHierarchyUser()) );
		users.Add( gameUtility.CreateNewUser(EventFlag.UserType.Enemy, gameUtility.CreateHierarchyUser()) );
	}

	public void LoadAllUnitToMap() {
		map.ClearUnits();
		GameObject prefab = Resources.Load<GameObject>("Prefab/Game/unit");
		
		//Manaully generate unit
		List<UnitPlacementComponent> playerPlacement = map.placements.FindAll(x=>x.userType == EventFlag.UserType.Player && x.placementType == EventFlag.PlacementType.Unit);
		foreach (UnitPlacementComponent p in playerPlacement) {
			map.GenerateUnitToPos(player, prefab, p);
		}

		//Enemy unit
		List<UnitPlacementComponent> enemyPlacement = map.placements.FindAll(x=>x.userType == EventFlag.UserType.Enemy && x.placementType == EventFlag.PlacementType.Unit);
		foreach (UnitPlacementComponent p in enemyPlacement) {
			map.GenerateUnitToPos(enemy, prefab, p);
		}	
	}

	void GameStart() {
		round = 1;

		AssignPlayer();
		LoadAllUnitToMap();

		currentUser = users[0];

		//Load Character here
		//unitGenerator.GenerateUnitToPos();


		RoundStart();
	}

	void RoundStart() {
		currentUser.StartTurn();
	}

	void RoundEnd() {
		round++;
		RoundStart();
	}

	//Click Event
	public void EndTurn() {
		currentUser.EndTurn();
		int index = users.IndexOf(currentUser);

		if (users.Count - 1 == index) {
			currentUser = users[0];
			RoundEnd();
		} else {
			currentUser = users[index+1];
			currentUser.StartTurn();
		}
	}

	public void GameOver() {

	}  

}
