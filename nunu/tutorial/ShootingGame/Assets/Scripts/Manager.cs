using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	public GameObject player;
	public GameObject heatGauge;
	public GameObject score;

	public GameObject title;
	public GameObject gameOver;

	public enum mode_tag {
		TITLE,
		PLAYING,
		GAMEOVER
	}
	private mode_tag gameMode;


	// Use this for initialization
	void Start () {
		ShowTitle();
	}

	
	// Update is called once per frame
	void Update() {

		switch (gameMode) {
		case mode_tag.PLAYING:
			if (Input.GetMouseButtonDown(1)) {
				//FindObjectOfType<Player>().GetComponent<Spaceship>().Explosion();
				//Destroy(FindObjectOfType<Player>().gameObject);
				GameOver();
			}
			break;

		case mode_tag.TITLE:
			//タップorクリックorＸキーでゲーム開始
			if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0)) {
				GameStart();
			} else {
				for (int i = 0; i < Input.touchCount; i++) {
					Touch touch = Input.GetTouch(i);
					if (touch.phase == TouchPhase.Began) {
						GameStart();
						break;
					}
				}
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
	
	/*
	void OnGUI() {
		//Eventクラスを使ってクリックやタップを検知する方法
		if (!IsPlaying() && Event.current.type == EventType.MouseDown) {
			GameStart();
		}
	}
	*/

	void GameStart() {
		gameMode = mode_tag.PLAYING;
		title.SetActive(false);
		heatGauge.SetActive(true);
		Instantiate(player, player.transform.position, player.transform.rotation);
	}

	public void GameOver() {
		gameMode = mode_tag.GAMEOVER;
		
		FindObjectOfType<Score>().Save();
		gameOver.transform.Find("Score").GetComponent<GUIText>().text = "Score " + FindObjectOfType<Score>().GetScore().ToString();

		while (true) {
			heatGauge.SetActive(false);
			gameOver.SetActive(true);
		}
	}

	private void ShowTitle() {
		gameMode = mode_tag.TITLE;
		title.SetActive(true);
		gameOver.SetActive(false);
		heatGauge.SetActive(false);
	}

	public bool IsPlaying() {
		return (gameMode == mode_tag.PLAYING);
	}
}
