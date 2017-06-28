using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Player {
	public class User : MonoBehaviour {
		public List<Unit> allUnits = new List<Unit>();
		protected GameManager gm;
		public EventFlag.UserType userType;

		public virtual void PreLoad(GameManager p_gManager, EventFlag.UserType p_uType) {
			gm = p_gManager;
			userType = p_uType;
		}


		public virtual void GenerateCharacters( List<UnitPlacementComponent> placements ) {
			GameObject prefab = Resources.Load<GameObject>("Prefab/Game/unit");

			foreach (UnitPlacementComponent p in placements) {
				string unitID = p.propertyJSON.GetField("id").str;
				CharacterPrefab characterPrefab = gm.database.FindByID<CharacterPrefab>(unitID);
				JSONObject characterJSON = GetCharacter( characterPrefab );

				Unit unit = gm.map.GenerateUnitToPos(prefab, p, characterPrefab);

				unit.Set(this, characterPrefab._class.ToLower(), characterPrefab,  characterJSON );
				allUnits.Add( unit );

			}
		}

		public virtual JSONObject GetCharacter( CharacterPrefab p_character) {
			return gm.mechanism.characterManager.GetCharacterJSON(p_character, true);
		}

		public virtual void StartTurn() {
			foreach (Unit unit in allUnits ) {
				unit.status = Unit.Status.Idle;
				unit.GetComponent<SpriteRenderer>().color= Color.white;
				unit.UnitHighLightControl(true);
			}
		}

		public virtual void EndTurn() {
			foreach (Unit unit in allUnits ) {
				unit.UnitHighLightControl(false);
			}
		}

	}
}