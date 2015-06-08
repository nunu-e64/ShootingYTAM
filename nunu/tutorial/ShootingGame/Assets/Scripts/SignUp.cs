using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SignUp : MonoBehaviour {

	public Button button;
	public InputField inputField;

	private string userNameKey = "userName";	//PlayerPrefsで保存するためのキー;
	private string userName = "";

	void Start () {
		Initialize ();
	}

	//初期化…ローカルデータチェックとInputField,Buttonの初期化
	public void Initialize () {
		userName = PlayerPrefs.GetString (userNameKey, "");		//端末から登録名を取得
		if (userName.Length > 0) {
			FindObjectOfType<Manager>().SignUpComplete();
		} else {
			inputField.text = "";
			ChangeButtonStyle (false);
		}
	}

	void OnGUI () {
		//Vector2 pos = Camera.main.ViewportToScreenPoint (new Vector2(idText.transform.position.x, 1.0f - idText.transform.position.y));
		//Rect rect1 = new Rect (pos.x + 10, pos.y - 20, 120, 40);
		//userName = GUI.TextField (rect1, userName, 8, inputStyle);
	}

	public string GetUserName () {
		return userName;
	}

	//uGUI(InputField).EndEditから呼び出し
	public void CheckInputField () {
		userName = inputField.text;
		Debug.Log (userName);
		ChangeButtonStyle (userName.Length > 0);
	}

	private void ChangeButtonStyle(bool isOk){
		if (isOk) {
			button.enabled = true;
			button.GetComponentInChildren<Text> ().text = "Play";
			button.GetComponentInChildren<Text> ().color = Color.white;
		} else {
			button.enabled = false;
			button.GetComponentInChildren<Text> ().text = "Input ID";
			button.GetComponentInChildren<Text> ().color = Color.gray;
		}

	}

}
