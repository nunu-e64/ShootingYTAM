using UnityEngine;
using System.Collections;

public class SignUp : MonoBehaviour {

	private string userName;

	void OnGUI () {
		Rect rect1 = new Rect (10, 10, 400, 30);
		userName = GUI.TextField (rect1, userName, 8);
	}
}
