using UnityEngine;
using System.Collections;
using Player;
using Utility;

public class GameUtility {
	GameManager gameManager;

	public GameUtility (GameManager p_gameManager) {
		gameManager = p_gameManager;
	}

	public GameObject CreateHierarchyUser() {
		Transform userHolder = gameManager.transform.Find("user");
		GameObject userObject = new GameObject();
		userObject.name = "user";
		userObject.transform.SetParent(userHolder);
		return userObject;
	}

	public User CreateNewUser(EventFlag.UserType p_type, GameObject userObject) {
		userObject.name = p_type.ToString("g");
		User user;

		switch(p_type) {
			case EventFlag.UserType.Player :
				user = userObject.AddComponent<User>();
			break;

			case EventFlag.UserType.Enemy:
				user=userObject.AddComponent<AIManager>();
			break;

			default :
				user = userObject.AddComponent<User>();
			break;
		}

		user.PreLoad(gameManager, p_type);	
		return user;
	}
}
