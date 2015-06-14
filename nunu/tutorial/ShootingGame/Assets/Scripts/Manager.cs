using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ゲーム状況の管理クラス
/// </summary>
public class Manager : MonoBehaviour {

	[HeaderAttribute ("Prefabs")]
	public GameObject playerPrefab;		//GameStartでPlayerを生成するためのPrefab

	[HeaderAttribute ("RefScript")]
	public ScoreManager scoreManager;
	public Emitter emitter;

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

		ShowSignUp ();
		if (signUp.GetComponent<SignUp> ().Initialize ()) {		//名前が登録済みか確認し登録済みならタイトル画面へ
			SignUpComplete ();
		}
	}


	void Update() {

		switch (gameMode) {
		case mode_tag.PLAYING:
			if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Escape)) {
				if (player) player.GetComponent<Player>().OnDead();
			}
			break;

		case mode_tag.TITLE:			//TIPS: 画面タップによるゲームスタートはButtonで実装している
			if (Input.GetKeyDown (KeyCode.X)) GameStart ();

			if (Input.GetKeyDown (KeyCode.Delete) || Input.GetMouseButtonDown (2)) {
				if (GameObject.Find("DeleteMessage").activeSelf) DeletePlayerPrefs();
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


	public void GameStart() {
		gameMode = mode_tag.PLAYING;
		title.SetActive(false);
		titleMessage.SetActive (false);
		gauge.SetActive(true);
		player = Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation) as GameObject;		//自機出現
	}

	public void GameOver() {
		gameMode = mode_tag.GAMEOVER;

		//スコア取得と表示とセーブ
		int score = scoreManager.GetScore ();
		gameOver.transform.Find("Score").GetComponent<Text>().text = "Score " + score.ToString();
		scoreManager.Save ();

		//TODO: ハイスコア更新時にはランキング送信//////////////////////////////////////////////////////////////
		if (scoreManager.IsHighScore ()) {
			//SendRanking(signUp.GetComponent<SignUp> ().UserName, score);
			Debug.Log ("SendRanking:" + signUp.GetComponent<SignUp> ().UserName + "," + score);
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
		
		gauge.SetActive(false);
		gameOver.SetActive(true);
	}

	private void ShowTitle() {
		gameMode = mode_tag.TITLE;
		title.SetActive (true);
		titleMessage.SetActive (true);
		gameOver.SetActive(false);
		gauge.SetActive(false);
		signUp.SetActive (false);

		scoreManager.Initialize ();
		emitter.Init ();
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
