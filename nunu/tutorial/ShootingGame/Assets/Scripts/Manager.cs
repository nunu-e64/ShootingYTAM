using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Manager : MonoBehaviour {

	public GameObject player;
	public GameObject gauge;
	public ScoreManager scoreManager;

	public GameObject title;
	public GameObject gameOver;
	public GameObject signUp;
	public GameObject titleMessage;

	public enum mode_tag {
		SIGNUP,
		TITLE,
		PLAYING,
		GAMEOVER
	}
	private mode_tag gameMode;


	// Use this for initialization
	void Start () {

		ShowSignUp ();
		if (signUp.GetComponent<SignUp> ().Initialize ()) {
			SignUpComplete ();
		}
	}

	
	// Update is called once per frame
	void Update() {

		switch (gameMode) {
		case mode_tag.PLAYING:
			if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Escape)) {
				FindObjectOfType<Player>().GetComponent<Spaceship>().Explosion();
				Destroy(FindObjectOfType<Player>().gameObject);
				GameOver();
			}
			break;

		case mode_tag.TITLE:
			//uGUI(Button)で管理
			////タップorクリックorＸキーでゲーム開始
			//if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0)) {
			//	GameStart();
			//} else {
			//	for (int i = 0; i < Input.touchCount; i++) {
			//		Touch touch = Input.GetTouch(i);
			//		if (touch.phase == TouchPhase.Began) {
			//			GameStart();
			//			break;
			//		}
			//	}
			//}
			//
			if (Input.GetKeyDown (KeyCode.X)) GameStart ();

			if (Input.GetKeyDown (KeyCode.Delete) || Input.GetMouseButtonDown (2)) {
				if (GameObject.Find("DeleteMessage").activeSelf) DeletePlayerPrefs();
			}
			break;

		case mode_tag.GAMEOVER:
			//タップorクリックorＸキーでタイトルに戻る
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
		Instantiate(player, player.transform.position, player.transform.rotation);
	}

	public void GameOver() {
		gameMode = mode_tag.GAMEOVER;

		int score = scoreManager.GetScore ();
		gameOver.transform.Find("Score").GetComponent<Text>().text = "Score " + score.ToString();
		scoreManager.Save ();

		//ハイスコア更新時にはランキング送信
		if (scoreManager.IsHighScore ()) {
			//SendRanking(signUp.GetComponent<SignUp> ().UserName, score);
			Debug.Log ("SendRanking:" + signUp.GetComponent<SignUp> ().UserName + "," + score);
		}
		
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
		FindObjectOfType<ScoreManager> ().Initialize ();
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
		FindObjectOfType<ScoreManager> ().Initialize ();
		ShowSignUp ();
		signUp.GetComponent<SignUp> ().Initialize ();
	}


	//ランキング表示シーンに遷移
	public void GoToRanking () {
		Application.LoadLevel ("Ranking");
	}

	//サインアップ完了を受け取る。登録済み:Start()から　新規登録：SignUp.NewNameSignUpから呼び出し
	public void SignUpComplete () {
		string userName = signUp.GetComponent<SignUp>().UserName;
		if (userName.Length == 0) {
			Debug.LogError("userName.Legnth = 0");	//DEBUG: チェックは他でしているが当面の予防的措置
		}
		
		titleMessage.transform.Find ("Name").GetComponent<Text> ().text = "ID : " + userName; 
		ShowTitle ();
	}
}
