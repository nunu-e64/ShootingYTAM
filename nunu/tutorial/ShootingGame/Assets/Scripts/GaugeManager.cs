using UnityEngine;
using System.Collections;

public class GaugeManager : MonoBehaviour {

	private Spaceship playerSpaceShip = null;
	private bool playerShotable;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (playerSpaceShip) playerSpaceShip.shotable = playerShotable;
	
	}

	public void SetPlayer(Spaceship ship) {
		playerSpaceShip = ship;
	}
}
