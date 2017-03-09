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