using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	public bool dontDestroyEnabled = true;

	private static Sound instance = null;
	
	void Awake () {
		 if (instance != null && instance != this) {
			 Destroy(this.gameObject);
			 return;
		 } else {
			 instance = this;
		 }
		 DontDestroyOnLoad(this.gameObject);
	}

}
