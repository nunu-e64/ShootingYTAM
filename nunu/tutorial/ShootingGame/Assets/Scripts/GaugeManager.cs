﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ゲージの状態によって自機が弾を打てるかを切り替えるための関数群
/// ・AnimationEventから呼び出す
/// </summary>
public class GaugeManager : MonoBehaviour {

	[SerializeField]
	private Text gaugeText;

	public float countDown = 0.00f;		//ゲージのカウントダウン表示。Animationで値変化

	private Player player = null;
	private bool isShooting;


	void Start () {	

		//Textの位置をゲージに合わせる
		gaugeText.transform.position = (Vector2) Camera.main.WorldToScreenPoint (transform.position);

	}


	void Update () {
		
		if (!isShooting) {
			string tmpText = countDown.ToString ("00.00");
			gaugeText.text = tmpText;
		}

	}

	void OnDisable () {
		if (!gaugeText.IsDestroyed()) gaugeText.gameObject.SetActive(false);
	}

	public void SetPlayer(Player _player) {
		player = _player;
	}

	public void StartShooting () {
		isShooting = true;
		player.shotable = true;
		gaugeText.gameObject.SetActive (false);
		player.InvokeSpecialAttack ();
	}

	public void StartCharging () {
		isShooting = false;
		player.shotable = false;
		gaugeText.gameObject.SetActive (true); 
	}
}
