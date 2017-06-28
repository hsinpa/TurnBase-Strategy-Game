using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CharacterManager : MonoBehaviour {

	public JSONObject GetCharacterJSON ( CharacterPrefab p_characterPrefab, bool loadFromSave) {
		SaveManager save = MainApp.Instance.game.save;
		JSONObject characterJSON = save.saveSlotJSON.GetField("Character");

		//Create a new character json
		if ( !characterJSON.HasField(p_characterPrefab._id) || !loadFromSave) {
			JSONObject newCharacter = new JSONObject();
			newCharacter.SetField("name", p_characterPrefab._name);
			newCharacter.SetField("_id", p_characterPrefab._id);

			newCharacter.SetField("level", 1);
			newCharacter.SetField("hp", p_characterPrefab._hp);
			newCharacter.SetField("strength", p_characterPrefab._strength);
			newCharacter.SetField("defense", p_characterPrefab._defense);
			newCharacter.SetField("speed", p_characterPrefab._speed);
			newCharacter.SetField("skill", p_characterPrefab._skill);
			newCharacter.SetField("foot speed", p_characterPrefab._footspeed);

			return newCharacter;
		} else {
			return characterJSON.GetField(p_characterPrefab._id);
		}
	}

	public JSONObject LevelUp( CharacterPrefab p_characterPrefab, JSONObject characterJSON, int p_numOfLevel) {

		int strength = characterJSON.GetField("strength").num,
			defense = characterJSON.GetField("defense").num,
			speed = characterJSON.GetField("speed").num,
			skill = characterJSON.GetField("skill").num,
			hp  = characterJSON.GetField("hp").num;

		//Loop of number of level this character leveling up
		for (int i = 0; i < p_numOfLevel; i++ ) {

			if (UtilityMethod.PercentageGame( p_characterPrefab._strength_growth_rate * p_characterPrefab._character_growth_rate )) characterJSON.SetField("strength", strength++);
			if (UtilityMethod.PercentageGame( p_characterPrefab._defense_growth_rate * p_characterPrefab._character_growth_rate )) characterJSON.SetField("defense", defense++);
			if (UtilityMethod.PercentageGame( p_characterPrefab._speed_growth_rate * p_characterPrefab._character_growth_rate )) characterJSON.SetField("speed", speed++);
			if (UtilityMethod.PercentageGame( p_characterPrefab._skill_growth_rate * p_characterPrefab._character_growth_rate )) characterJSON.SetField("skill", skill++);
			if (UtilityMethod.PercentageGame( p_characterPrefab._hp_growth_rate * p_characterPrefab._character_growth_rate )) characterJSON.SetField("hp", hp++);

		}
		
		return characterJSON;
	}
}
