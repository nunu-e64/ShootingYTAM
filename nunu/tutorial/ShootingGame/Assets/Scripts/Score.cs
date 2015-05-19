using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public GUIText scoreGUIText;
	public GUIText highScoreGUIText;

	private int score;
	private int highScore;

	//PlayerPrefsで保存するためのキー
	private string highScoreKey = "highScore";


	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {

		if (score > highScore) {
			highScore = score;
		}

		scoreGUIText.text = score.ToString();
		highScoreGUIText.text = "HighScore:" + highScore.ToString();
	}

	private void Initialize() {
		score = 0;
		highScore = PlayerPrefs.GetInt(highScoreKey, 0);
	}

	public void AddPoint(int point) {
		score += point;
	}

	public void Save() {
		PlayerPrefs.SetInt(highScoreKey, highScore);
		PlayerPrefs.Save();

		Initialize();
	}
}
