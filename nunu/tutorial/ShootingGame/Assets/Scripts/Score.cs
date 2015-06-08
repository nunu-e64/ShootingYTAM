﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

	public Text scoreText;
	public Text highScoreText;
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
	

	public void Initialize() {
		score = 0;
		highScore = PlayerPrefs.GetInt(highScoreKey, 0);

		scoreText.text = score.ToString();
		highScoreText.text = highScore.ToString();
		scoreText.color = Color.white;
		highScoreText.color = Color.white;
	}

	public void AddPoint(int point) {
		score += point;

		if (score > highScore) {
			highScore = score;
			scoreText.color = Color.red;
			highScoreText.color = Color.red;
		}
		scoreText.text = score.ToString();
		highScoreText.text = highScore.ToString();
	}

	public void Save() {
		PlayerPrefs.SetInt(highScoreKey, highScore);
		PlayerPrefs.Save();
	}

	public int GetScore() {
		return score;
	}
}
