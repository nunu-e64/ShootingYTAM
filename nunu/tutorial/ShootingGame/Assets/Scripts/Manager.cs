using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ゲーム状況の管理クラス
/// </summary>
public class Manager : MonoBehaviour {

	[HeaderAttribute ("DebugSwitch")]
	public bool isDebug = false;		//デバッグ表示切替用スイッチ

	[HeaderAttribute ("Prefabs")]
	public GameObject playerPrefab;		//GameStartでPlayerを生成するためのPrefab

	[HeaderAttribute ("Managers")]
	public ScoreManager scoreManager;
	public StageManager stageManager;

	[HeaderAttribute ("表示切替用Object")]
	public GameObject title;			//ゲーム状況（mode）に応じて表示切替するための各種UI
	public GameObject gameOver;			//
	public GameObject signUp;			//
	public GameObject titleMessage;		//
	public GameObject gauge;			//

	private GameObject player;			//生成したPlayer

	private enum mode_tag {
		SIGNUP,
		TITLE,
		PLAYING,
		GAMEOVER
	}
	private mode_tag gameMode;


	void Start () {

		foreach (GameObject item in GameObject.FindGameObjectsWithTag ("Debug")) {
			if (!isDebug) Debug.LogError ("Object Tagged 'Debug' is acitve.:" + item);
		}
	
		ShowSignUp ();
		if (signUp.GetComponent<SignUp> ().Initialize ()) {		//名前が登録済みか確認し登録済みならタイトル画面へ
			SignUpComplete ();
		}
	}


	void Update() {

		switch (gameMode) {
		case mode_tag.PLAYING:
			break;

		case mode_tag.TITLE:			//TIPS: 画面タップによるゲームスタートはButtonで実装している
			if (Input.GetKeyDown (KeyCode.X)) GameStart ();

			if (isDebug && (Input.GetKeyDown (KeyCode.Delete) || Input.GetMouseButtonDown (2))) {
				DeletePlayerPrefs();
			}
			break;

		case mode_tag.GAMEOVER:
			if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0)) {
				ShowTitle();
			} else {
				for (int i = 0; i < Input.touchCount; i++) {
					Touch touch = Input.GetTouch(i);
					if (touch.phase == TouchPhase.Began) {
						ShowTitle();
						break;
					}
				}
			}
			break;
		}
	}


	public void GameStart () {
		gameMode = mode_tag.PLAYING;
		title.SetActive (false);
		titleMessage.SetActive (false);
		gauge.SetActive (true);
		player = Instantiate (playerPrefab) as GameObject;		//自機出現
		Vector3 pos = Camera.main.ViewportToWorldPoint (new Vector2 (0.5f, 0));
		player.transform.position = new Vector3 (pos.x, pos.y, 0);
	}

	public IEnumerator GameOver() {
		Debug.Log ("開始");
		yield return new WaitForSeconds (2.0f);
		Debug.Log ("Back");

		gameMode = mode_tag.GAMEOVER;

		//スコア取得と表示とセーブ
		int score = scoreManager.GetScore ();
		gameOver.transform.Find("Score").GetComponent<Text>().text = "Score " + score.ToString();
		scoreManager.Save ();

		//TODO: ハイスコア更新時にはランキング送信//////////////////////////////////////////////////////////////
		if (true) {
			//SendRanking(signUp.GetComponent<SignUp> ().UserName, score);
			StartCoroutine (SendUserScore (score));
			Debug.Log ("RequestSendingScoreToRanking:" + signUp.GetComponent<SignUp> ().UserName + "," + score);
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		gauge.SetActive (false);
		gameOver.SetActive(true);
	}
	IEnumerator SendUserScore(int score){
		string userId = signUp.GetComponent<SignUp>().UserId;
		Debug.Log ("userId:"+ userId);
		Debug.Log ("score:"+ score);
		string url = "http://localhost/cakephp/ranking/Scores/scoreAdd?user_id="+ userId + "&score="+score.ToString();
		WWW www = new WWW (url);

		yield return www;

		if (www.error != null) {
			Debug.LogWarning ("WWWERROR: " + www.error);
			yield break;
		} else if (!www.isDone) {
			Debug.LogWarning ("WWWERROR: " + "UNDONE");
			yield break;
		} else  if (www.text == "false") {
			Debug.LogWarning ("WWWERROR: Failed");
			yield break;
		}else{
			Debug.Log ("Success");
		}
	}
	private void ShowTitle() {
		gameMode = mode_tag.TITLE;
		title.SetActive (true);
		titleMessage.SetActive (true);
		gameOver.SetActive(false);
		gauge.SetActive (false);
		signUp.SetActive (false);

		scoreManager.Initialize ();
		stageManager.Init ();
	}

	private void ShowSignUp () {
		gameMode = mode_tag.SIGNUP;
		title.SetActive (true);
		titleMessage.SetActive (false);
		gameOver.SetActive (false);
		gauge.SetActive (false);
		signUp.SetActive (true);
	}


	public bool IsPlaying() {
		return (gameMode == mode_tag.PLAYING);
	}

	private void DeletePlayerPrefs () {
		PlayerPrefs.DeleteAll ();
		scoreManager.Initialize ();
		ShowSignUp ();
		signUp.GetComponent<SignUp> ().Initialize ();
	}


	//ランキング表示シーンに遷移
	public void GoToRanking () {
		Application.LoadLevel ("Ranking");
	}

	//サインアップ完了を受け取る。登録済み:Start()から呼び出し　新規登録：SignUp.NewNameSignUpから呼び出し
	public void SignUpComplete () {
		string userName = signUp.GetComponent<SignUp>().UserName;
		if (userName.Length == 0) {
			Debug.LogError("userName.Legnth = 0");	//DEBUG: チェックは他でしているが当面の予防的措置
		}
		
		titleMessage.transform.Find ("Name").GetComponent<Text> ().text = "ID : " + userName; 
		ShowTitle ();
	}
}
