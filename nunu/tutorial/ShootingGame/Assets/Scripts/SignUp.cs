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
	public Text message1;
	public Text message2;

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
			message1.text = "Input Your Handle Name.";
			message2.text = "半角英数8文字以内で\nあなたの名前を入力してください";
			message1.color = new Color (170f / 255, 170f / 255, 170f / 255);
			message2.color = new Color (170f / 255, 170f / 255, 170f / 255);
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
		message1.text = "   Connecting To Server...";
		message2.text = "   サーバー接続中...";
		message1.color = new Color (180f / 255, 170f / 255, 0f / 255);
		message2.color = new Color (180f / 255, 170f / 255, 0f / 255);
		
		WWW www = new WWW (url);

		yield return www;

		if (www.error != null && !GameObject.FindObjectOfType<Manager> ().isDebug) {
			Debug.LogWarning ("WWWERROR: " + www.error);
			message1.text = www.error;
			message2.text = "通信エラー";
			message1.color = new Color (230f / 255, 70f / 255, 70f / 255);
			message2.color = new Color (230f / 255, 70f / 255, 70f / 255);
			yield break;

		} else if (!www.isDone && !GameObject.FindObjectOfType<Manager> ().isDebug) {
			Debug.LogWarning ("WWWERROR: " + "UNDONE");
			message1.text = "Connection Error";
			message2.text = "通信エラー";
			message1.color = new Color (230f / 255, 70f / 255, 70f / 255);
			message2.color = new Color (230f / 255, 70f / 255, 70f / 255);
			yield break;

		//名前が重複している場合ユーザ登録できない
		}else if (www.text == "false") {
			//名前が重複しておりIDが与えられない。
			Debug.Log ("Already Existed Name.");
			message1.text = "Already Exist ID.";
			message2.text = "この名前は既に利用されています。\n他の名前を入力してください";
			message1.color = new Color (230f/255, 70f/255, 70f/255);
			message2.color = new Color (230f/255, 70f/255, 70f/255);
			yield break;

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

