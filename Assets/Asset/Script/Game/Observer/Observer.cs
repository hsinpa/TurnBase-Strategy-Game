using UnityEngine;
using System.Collections;

namespace ObserverPattenr {
	public class Observer : MonoBehaviour {

		public virtual void OnNotify(string p_event, params object[] p_objects) {
			
		}
	}
}