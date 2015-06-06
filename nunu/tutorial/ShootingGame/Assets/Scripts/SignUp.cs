using UnityEngine;
using System.Collections;

public class SignUp : MonoBehaviour {

	public GUIStyle inputStyle;
	public GameObject idText;
	public GameObject signUp;
	public GameObject button;

	private string userNameKey = "userName";	//PlayerPrefsで保存するためのキー;
	private string userName = "";
	private bool ok = false;	//登録済みか否か

	void Start () {
		Vector2 pos = Camera.main.ViewportToWorldPoint (new Vector2 (signUp.transform.position.x, signUp.transform.position.y));
		button.transform.position = pos;
		Initialize ();
	}

	public void Initialize () {
		userName = PlayerPrefs.GetString (userNameKey, "");		//端末から登録名を取得
		if (userName.Length > 0) {
			ok = true;
		} else {
			ok = false;	//登録済みだったときはすぐにTitleに遷移
		}
	}

	void OnGUI () {
		Vector2 pos = Camera.main.ViewportToScreenPoint (new Vector2(idText.transform.position.x, 1.0f - idText.transform.position.y));
		Rect rect1 = new Rect (pos.x + 10, pos.y - 20, 120, 40);
		userName = GUI.TextField (rect1, userName, 8, inputStyle);

		if (userName.Length == 0) {
			signUp.GetComponent<GUIText> ().text = "Input ID";
			signUp.GetComponent<GUIText> ().color = Color.grey;
		} else {
			signUp.GetComponent<GUIText> ().text = "Play";
			signUp.GetComponent<GUIText> ().color = Color.white;
		}
	}

	void Update () {

		if (!ok && userName.Length > 0) {
			Ray ray;
			if (Input.GetMouseButtonDown (0)) {
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			} else if (Input.touchCount > 0) {
				ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			} else {
				return;
			}

			RaycastHit hit = new RaycastHit ();

			if (Physics.Raycast (ray, out hit)) {
				if (button == hit.collider.gameObject) {
					ok = true;
					PlayerPrefs.SetString (userNameKey, userName);
					PlayerPrefs.Save ();
				}
			}
		}
	}

	public string GetUserName () {
		if (ok) return userName; else return "";
	}

}
