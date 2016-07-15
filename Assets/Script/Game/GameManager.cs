using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Player;

public class GameManager : MonoBehaviour {
	public enum Turn {PlayerTurn, EnemyTurn};

	public int round = 1;
	public List<User> users = new List<User>();
	public Map map;
	public User player {get { return users[0]; } }
	public User enemy {get { return users[1]; } }

	//User on its turn
	public User currentUser;
	public InputManager inputManager;

	// Use this for initializationy
	void Awake () {
		//Load Users
		users.Add(transform.FindChild("user/player").GetComponent<User>());
		users.Add(transform.FindChild("user/enemy").GetComponent<AIManager>());

		inputManager = transform.FindChild("user").GetComponent<InputManager>();
		map = GetComponentInChildren<Map>(); 
		map.Load();
		GameStart();
	}

	void GameStart() {
		round = 1;
		currentUser = users[0];
		users.ForEach(delegate(User obj) {
			obj.PreLoad(this);	
		});

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
