using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ユーザー名管理クラス
/// ・新規登録ルーチン
/// ・ローカルへのユーザー名保存
/// ・ローカルからのユーザー名取得
/// </summary>
public class SignUp : MonoBehaviour {

	public Button button;
	public InputField inputField;

	private string userNameKey = "userName";	//PlayerPrefsで保存するためのキー;
	private string userIdKey = "userId";
	private string userName = "";
	private string userId = "";

	public string UserName {
		get { return userName; }
	}
	public string UserId {
		get { return userId; }
	}

	//初期化…Managerから呼び出す。ローカルデータチェックとInputField,Buttonの初期化
	public bool Initialize () {
		userName = PlayerPrefs.GetString (userNameKey, "");		//端末から登録名を取得
		userId = PlayerPrefs.GetString (userIdKey, "");
		if (userName.Length > 0) {	//登録済み
			return true;
		} else {
			inputField.text = "";	//未登録
			ChangeButtonStyle (false);
			return false;
		}
	}

	//変数とUIの同期	uGUI(InputField)から呼び出す
	public void CheckInputField () {
		userName = inputField.text;
		ChangeButtonStyle (userName.Length > 0);
	}

	//入力済みか否かでボタン表示を切り替え
	private void ChangeButtonStyle(bool isOk){
		if (isOk) {
			button.enabled = true;
			button.GetComponentInChildren<Text> ().color = Color.white;
		} else {
			button.enabled = false;
			button.GetComponentInChildren<Text> ().color = Color.gray;
		}

	}

	//新規登録完了	Buttonから呼び出す
	public void NewNameSignUp () {
		CheckInputField ();

		////////////////////////////////////////////////////////////////
		//TODO: DBにアクセスして既に存在している名前かチェックする
		//・重複(既に存在する)→メッセージを表示してreturn;
		//・ＯＫ(存在しない)→続行
		////////////////////////////////////////////////////////////////
		StartCoroutine (UserRegister ());
	}

	IEnumerator UserRegister(){
		string url = "http://localhost/cakephp/ranking/Users/userAdd?name=" + userName;
		WWW www = new WWW (url);

		yield return www;
		//名前が重複している場合ユーザ登録できない
		if (www.text == "false") {
			//名前が重複しておりIDが与えられない。
			Debug.Log ("名前が重複してます。");
//			UnityEditor.EditorUtility.DisplayDialog ("alert", "既に登録された名前です。", "ok!");

		} else {
			Debug.Log ("登録成功");
			userId = www.text as string;
			Debug.Log ("取得したID:" + userId);
			PlayerPrefs.SetString (userNameKey, userName);
			PlayerPrefs.SetString (userIdKey, userId);
			PlayerPrefs.Save ();
			FindObjectOfType<Manager> ().SignUpComplete ();
		}
    }
}