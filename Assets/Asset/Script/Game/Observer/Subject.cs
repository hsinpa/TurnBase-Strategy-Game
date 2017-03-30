using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ObserverPattenr {
	public class Subject {
		private List<Observer> mObservers = new List<Observer>();

		public void addObserver(Observer observer) {
		    // Add to array...
		    mObservers.Add(observer);
		}

		public void removeObserver(Observer observer) {
		    // Remove from array...
		    mObservers.Remove(observer);
		}

		public void notify( string entity, params object[] objects) {
			for (int i = 0; i < mObservers.Count; i++) {
				if (mObservers[i] == null) continue;
				mObservers[i].OnNotify(entity, objects);
		  }
		}

	}
}