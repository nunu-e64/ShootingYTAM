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
	public Text message;

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
            StartCoroutine(LogIn());
            Debug.Log("requested");
            return true;
		} else {
			inputField.text = "";	//未登録
			ChangeButtonStyle (false);
			message.text = "半角英数8文字以内で\nあなたの名前を入力してください";
			message.color = new Color (170f / 255, 170f / 255, 170f / 255);
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

    IEnumerator LogIn() {
        string url = GameObject.FindObjectOfType<Manager>().GetUrl();

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("keyword", "LogIn");
        wwwForm.AddField("user_id", userId);
        WWW www = new WWW(url, wwwForm);

        Debug.Log("start id:" + userId);
        yield return www;

        Debug.Log(www.text);

        if (www.error != null) {
            Debug.LogWarning("WWWERROR: " + www.error);
            yield break;
        } else if (!www.isDone) {
            Debug.LogWarning("WWWERROR: " + "UNDONE");
            yield break;
        } else {
            Debug.Log("LogIn:Success");
            yield break;
        }

    }

    IEnumerator UserRegister(){
        string url = GameObject.FindObjectOfType<Manager>().GetUrl();
        message.text = "   サーバー接続中...";
		message.color = new Color (180f / 255, 170f / 255, 0f / 255);


        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("keyword", "SignUp");
        wwwForm.AddField("user_name", userName);
        WWW www = new WWW (url, wwwForm);

		yield return www;

		Debug.Log (www.text);

		if (www.error != null && !GameObject.FindObjectOfType<Manager> ().isDebug) {
			Debug.LogWarning ("WWWERROR: " + www.error);
			message.text = "通信エラー\n" + www.error;
			message.color = new Color (230f / 255, 70f / 255, 70f / 255);
			yield break;

		} else if (!www.isDone && !GameObject.FindObjectOfType<Manager> ().isDebug) {
			Debug.LogWarning ("WWWERROR: " + "UNDONE");
			message.text = "通信エラー";
			message.color = new Color (230f / 255, 70f / 255, 70f / 255);
			yield break;

		//名前が重複している場合ユーザ登録できない
		} else if (www.text == "ng" && !GameObject.FindObjectOfType<Manager> ().isDebug) {
			//名前が重複しておりIDが与えられない。
			Debug.Log ("Already Existed Name.");
			message.text = "この名前は既に利用されています。\n他の名前を入力してください";
			message.color = new Color (230f/255, 70f/255, 70f/255);
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

