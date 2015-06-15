using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fps : MonoBehaviour {

	float counter = 0;
	const int FPS_CALC_INTERVAL = 60;

	float fps;
	float timer = 0;


	// Update is called once per frame
	void Update () {

		++counter;
		timer += Time.deltaTime;

		if (counter >= FPS_CALC_INTERVAL) {
			fps = FPS_CALC_INTERVAL/timer;
			GetComponent<Text> ().text = fps.ToString ("00.00");
			timer = 0;
			counter = 0;
		}

	}
}
