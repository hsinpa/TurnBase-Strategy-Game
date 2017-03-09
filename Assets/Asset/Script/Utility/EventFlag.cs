using UnityEngine;
using System.Collections;

public class EventFlag : MonoBehaviour {
	public class Game {
		public const string EnterGame = "game.enter@event";

		public const string GameStart = "game.start@event";
		public const string RoundStart = "game.round.start@event";
		public const string RoundEnd = "game.round.end@event";
		public const string GameEnd = "game.end@event";
	}


	public enum PlacementType {
		Unit,
		SpawnPoint,
	}

	public enum UserType {Player, Enemy, Ally };


}
