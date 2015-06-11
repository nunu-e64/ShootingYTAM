using UnityEngine;
using System.Collections;

/// <summary>
/// ゲージの状態によって自機が弾を打てるかを切り替えるための関数群
/// ・AnimationEventから呼び出す
/// </summary>
public class GaugeManager : MonoBehaviour {

	public float countDown = 0.00f;

	private Spaceship playerSpaceShip = null;
	private Animator animator;
	private bool isShooting;

	GUIText[] gaugeText;

	void Start () {	
		//GUITextの位置をゲージに合わせる
		gaugeText = GetComponentsInChildren<GUIText> ();
		foreach (GUIText item in gaugeText) {
			item.transform.position = Camera.main.WorldToViewportPoint (transform.position);
		}

		animator = GetComponent<Animator>();
	}


	void Update () {
		
		if (!isShooting) {
			string tmpText = countDown.ToString ("00.00");
			gaugeText[0].text = tmpText;
		}

	}

	public void SetPlayer(Spaceship ship) {
		playerSpaceShip = ship;
	}

	public void StartShooting () {
		isShooting = true;
		StartPlayerShot ();
	}

	public void StartCharging () {
		isShooting = false;
		StopPlayerShot ();
	}

	private void StartPlayerShot() {
		if (playerSpaceShip) playerSpaceShip.shotable = true; else Debug.LogError(playerSpaceShip);
		Debug.Log("StartPlayerShot");
	}
	private void StopPlayerShot() {
		if (playerSpaceShip) playerSpaceShip.shotable = false; else Debug.LogError(playerSpaceShip);
		Debug.Log ("StopPlayerShot");
	}
}
