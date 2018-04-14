using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton type to be inherited where needed
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	private static T instance;

	//Returns the instance of this singleton.
	public static T Instance {
		get {
			if (instance == null) {
				instance = (T)FindObjectOfType(typeof(T));

				if (instance == null) {
					return null;
				}
			}
			return instance;
		}
	}

	public virtual void Awake() {
		if (instance != null && instance != this) {
			//Debug.Log("Rejected new instance of " + this.name + "(" + gameObject.name + ")");
			Destroy(gameObject);
			return;
		}
		//Debug.Log("Creating instance of " + this.name + "(" + gameObject.name + ")");
	}

	public virtual void OnDestroy() {
		//Debug.Log("Destroying instance of " + this.name + "(" + gameObject.name + ")");
		instance = null;
	}
}
