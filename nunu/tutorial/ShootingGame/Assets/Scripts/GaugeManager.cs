using UnityEngine;
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

	private Spaceship playerSpaceShip = null;
	private bool isShooting;


	void Start () {	

		//Textの位置をゲージに合わせる
		Debug.Log (transform.position);
		Debug.Log (Camera.main.WorldToViewportPoint (transform.position));
		gaugeText.transform.position = (Vector2) Camera.main.WorldToScreenPoint (transform.position);

	}


	void Update () {
		
		if (!isShooting) {
			string tmpText = countDown.ToString ("00.00");
			gaugeText.text = tmpText;
		}

	}

	void OnDisable () {
		gaugeText.gameObject.SetActive(false);
	}

	public void SetPlayer(Spaceship ship) {
		playerSpaceShip = ship;
	}

	public void StartShooting () {
		isShooting = true;
		StartPlayerShot ();
		gaugeText.gameObject.SetActive (false);
	}

	public void StartCharging () {
		isShooting = false;
		StopPlayerShot ();
		gaugeText.gameObject.SetActive (true);
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
