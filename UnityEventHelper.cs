using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Handy {
	public class UnityEventHelper : MonoBehaviour {

		[Tooltip("When this GameObject is set active")]
		public UnityEvent onEnable;
		[Tooltip("When this GameObject is disabled")]
		public UnityEvent onDisable;
		[Tooltip("When this GameObject is done initializing internally and calls Start")]
		public UnityEvent onStart;
		[Tooltip("What to set EventSystem focus on when this object is enabled")]
		public GameObject focusOnEnable;
		[Tooltip("What to set EventSystem focus on when this object is disabled")]
		public GameObject focusOnDisable;
		
		void OnEnable(){
			onEnable.Invoke();
			if (focusOnEnable){
				SetSelectedFocus(focusOnEnable);
			}
		}

		void OnStart(){
			onStart.Invoke();
		}

		void OnDisable() {
			onDisable.Invoke();
			if (focusOnDisable){
				SetSelectedFocus(focusOnDisable);
			}
		}

		public void SetSelectedFocus(GameObject selected){
			GetEventSystem().SetSelectedGameObject(selected);
		}

		EventSystem e;

		EventSystem GetEventSystem(){
			if (e == null){
				e = FindObjectOfType<EventSystem>();
			}
			return e;
		}

		public void SetNewFocusOnDisable(GameObject selected){
			focusOnDisable = selected;
		}

		public void SetNewFocusOnEnable(GameObject selected){
			focusOnEnable = selected;
		}
	}
}