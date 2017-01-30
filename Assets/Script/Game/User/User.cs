using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Player {
	public class User : MonoBehaviour {
		
		public List<Unit> allUnits;
		protected GameManager gm;

		public virtual void PreLoad(GameManager p_gManager) {
			gm = p_gManager;
			allUnits.ForEach(delegate(Unit obj) {
				obj.Set(this);
			});

		}


		public virtual void StartTurn() {
			foreach (Unit unit in allUnits ) {
				unit.status = Unit.Status.Idle;
				unit.GetComponent<SpriteRenderer>().color= Color.white;
			}
		}

		public virtual void EndTurn() {
			
		}

	}
}