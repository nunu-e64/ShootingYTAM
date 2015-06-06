using UnityEngine;
using System.Collections;


public class GaugeManager : MonoBehaviour {
	private Spaceship playerSpaceShip = null;

	public void SetPlayer(Spaceship ship) {
		playerSpaceShip = ship;
	}

	public void StartPlayerShot() {
		if (playerSpaceShip) playerSpaceShip.shotable = true; else Debug.LogError(playerSpaceShip);
		Debug.Log("StartPlayerShot");
	}
	public void StopPlayerShot() {
		if (playerSpaceShip) playerSpaceShip.shotable = false; else Debug.LogError(playerSpaceShip);
		Debug.Log ("StopPlayerShot");
	}
}
