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
	public CameraTransition mCamera { get { return transform.Find("camera").GetComponent<CameraTransition>(); }  }
	public DatabaseManager database { get { return transform.Find("database").GetComponent<DatabaseManager>(); }  }
	public GameStateManager gameState { get { return transform.GetComponent<GameStateManager>(); }  }

	public GameUtility gameUtility;

	private LevelPrefab mLevelPrefab;
	public MapPrefab mMapPrefab;
	private int mMapIndex;

	public override void OnNotify (string p_event, params object[] p_objects) {
		switch(p_event) {
			case EventFlag.Game.SetUp :
				mCamera.mode = CameraTransition.Mode.Show;

				int testLevel = 1;
				if (MainApp.Instance.stringTag.tagList.Count > 0) testLevel = int.Parse( MainApp.Instance.stringTag.tagList[0] );

				LevelPrefab levelPrefab = database.FindLevel(testLevel);

				SetUp( levelPrefab );
			break;

			case EventFlag.Game.EnterGame :

				//Set Map information
				mMapIndex = 0;
				mMapPrefab = mLevelPrefab._mapList[mMapIndex];

				round = 1;
				AssignPlayer();
				

				map.LoadMap( mMapPrefab._id );

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

			case EventFlag.Game.NextMap :
				mMapIndex++;
				if ( mMapIndex >=  mLevelPrefab._mapList.Count) {
					OnNotify(EventFlag.Game.GameEnd);
					return;
				} 
				mMapPrefab = mLevelPrefab._mapList[mMapIndex];

				ToNextMap(mMapPrefab);
			break;


			case EventFlag.Game.GameEnd :
				GameOver();
			break;
		}
	}

	//Prepare anything prequisted ready
	public void SetUp(LevelPrefab p_levelPrefab) {
		mLevelPrefab = p_levelPrefab;
		map = transform.Find("map").GetComponent<Map>(); 
		map.SetUp(  );

		inputManager = transform.FindChild("user").GetComponent<InputManager>();
		inputManager.SetUp(this, MainApp.Instance.ui);
		
		gameUtility = new GameUtility( this );
		uiManager = MainApp.Instance.ui;

		//Enable UI Canvas
		uiManager.GetComponent<Canvas>().enabled = true;		
	}

	public void AssignPlayer() {
		users.Add( gameUtility.CreateNewUser(EventFlag.UserType.Player, gameUtility.CreateHierarchyUser()) );
		users.Add( gameUtility.CreateNewUser(EventFlag.UserType.Enemy, gameUtility.CreateHierarchyUser()) );
	}

	public void LoadAllUnitToMap() {
		map.ClearUnits();
		player.allUnits.Clear();
		enemy.allUnits.Clear();

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

	public void ToNextMap(MapPrefab p_mapPrefab) {
		map.LoadMap( p_mapPrefab._id );
		GameStart();
	}

	void GameStart() {
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
