using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	public GameObject player;
	public GameObject heatGauge;
	public GameObject score;

	private GameObject title;



	// Use this for initialization
	void Start () {
		title = GameObject.Find("Title");
		heatGauge.SetActive(false);
	}

	
	// Update is called once per frame
	void Update () {
		//Touchクラスを使ってタップを検知する方法
		//タップでゲームスタート
		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch(i);
			if (!IsPlaying() && touch.phase == TouchPhase.Began) {
				GameStart();
			}
		}
		
		//Xキーかクリックでもゲームスタート
		if (!IsPlaying() && (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0)) ){
			GameStart();
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
		title.SetActive(false);
		heatGauge.SetActive(true);
		Instantiate(player, player.transform.position, player.transform.rotation);
	}

	public void GameOver() {
		FindObjectOfType<Score>().Save();
		heatGauge.SetActive(false);
		title.SetActive(true);
	}

	public bool IsPlaying() {
		return (!title.activeSelf);
	}
}
