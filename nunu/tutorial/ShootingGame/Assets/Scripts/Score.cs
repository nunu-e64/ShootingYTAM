using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public GUIText scoreGUIText;
	public GUIText highScoreGUIText;
	public int timeBonusPoint;
	public float timeBonusPeriod;

	private int score;
	private int highScore;
	private Manager manager;

	private string highScoreKey = "highScore";	//PlayerPrefsで保存するためのキー;


	// Use this for initialization
	IEnumerator Start () {
		manager = FindObjectOfType<Manager>();
		Initialize();

		if (timeBonusPoint > 0 && timeBonusPeriod > 0) {
			while (true) {
				if (manager.IsPlaying()) {
					AddPoint(timeBonusPoint);
				}

				yield return new WaitForSeconds(timeBonusPeriod);
			}
		}
	}
	

	private void Initialize() {
		score = 0;
		highScore = PlayerPrefs.GetInt(highScoreKey, 0);

		scoreGUIText.text = score.ToString();
		highScoreGUIText.text = highScore.ToString();
	}

	public void AddPoint(int point) {
		score += point;

		if (score > highScore) {
			highScore = score;
		}
		scoreGUIText.text = score.ToString();
		highScoreGUIText.text = highScore.ToString();
	}

	public void Save() {
		PlayerPrefs.SetInt(highScoreKey, highScore);
		PlayerPrefs.Save();

		Initialize();
	}

	public int GetScore() {
		return score;
	}
}
