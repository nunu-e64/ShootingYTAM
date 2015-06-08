using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SignUp : MonoBehaviour {

	public Button button;
	public InputField inputField;

	private string userNameKey = "userName";	//PlayerPrefsで保存するためのキー;
	private string userName = "";

	public string UserName {
		get { return userName; }
	}

	//初期化…Managerから呼び出す。ローカルデータチェックとInputField,Buttonの初期化
	public bool Initialize () {
		userName = PlayerPrefs.GetString (userNameKey, "");		//端末から登録名を取得
		if (userName.Length > 0) {
			return true;
		} else {
			inputField.text = "";
			ChangeButtonStyle (false);
			return false;
		}
	}


	//uGUI(InputField).EndEditからInputFieldの編集が終わるごとに呼び出し
	public void CheckInputField () {
		userName = inputField.text;
		ChangeButtonStyle (userName.Length > 0);
	}

	private void ChangeButtonStyle(bool isOk){
		if (isOk) {
			button.enabled = true;
			button.GetComponentInChildren<Text> ().color = Color.white;
		} else {
			button.enabled = false;
			button.GetComponentInChildren<Text> ().color = Color.gray;
		}

	}

	public void NewNameSignUp () {
		CheckInputField ();

		PlayerPrefs.SetString (userNameKey, userName);
		PlayerPrefs.Save ();
		FindObjectOfType<Manager> ().SignUpComplete ();
	}
}
