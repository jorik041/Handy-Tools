using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Handy {
	/// <summary>
	/// Used for passing events on from one object to another, EG: Animation triggered event to a script on a separate object
	/// </summary>
	public class PassOnEvent : MonoBehaviour 
	{
		public delegate void EventFired();
		public event EventFired OnEventFired;

		public void FireEvent() {
			if (OnEventFired != null) {
				OnEventFired();
			}
		}

		public delegate void EventFiredNumbered(int number);
		public event EventFiredNumbered OnEventFiredNumbered;

		public void FireNumberedEvent(int number) {
			if (OnEventFiredNumbered != null) {
				OnEventFiredNumbered(number);
			}
		}
	}
}