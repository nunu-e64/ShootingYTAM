using UnityEngine;
using System.Collections;


public class GaugeManager : MonoBehaviour {
	public bool hoge;

	private Spaceship playerSpaceShip = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		//if (playerSpaceShip) playerSpaceShip.shotable = playerShotable;
	
	}

	public void Hoge() {
	}
	void Foo() {
	}


	public void SetPlayer(Spaceship ship) {
		playerSpaceShip = ship;
	}

	public void StartPlayerShot() {
		if (playerSpaceShip) playerSpaceShip.shotable = true; else Debug.LogError(playerSpaceShip);
		Debug.Log("StartPlayerShot");
	}
	public void StopPlayerShot() {
		if (playerSpaceShip) playerSpaceShip.shotable = false; else Debug.LogError(playerSpaceShip);
		Debug.Log("StopPlayerShot");
	}
}
