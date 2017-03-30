using UnityEngine;
using System.Collections;

public class EventFlag : MonoBehaviour {
	public class Game {
		public const string SetUp = "game.setup@event";
		public const string EnterGame = "game.enter@event";

		public const string GameStart = "game.start@event";
		public const string RoundStart = "game.round.start@event";
		public const string RoundEnd = "game.round.end@event";
		public const string NextMap = "game.next@event";

		public const string GameEnd = "game.end@event";
	}

	public class GameUI {
		public const string SetUp = "gameui.setup@event";
	}

	public class MenuUI {
		public const string SetUp = "menuui.setup@event";
	}

	public enum PlacementType {
		Unit,
		SpawnPoint,
		TakeOff,
		Evacuation,
	}

	public enum UserType {Player, Enemy, Ally, Other };

	public enum WinCondition {KillAll, Occupy, Evacuation };

}
