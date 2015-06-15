using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fps : MonoBehaviour {

	float counter = 0;
	const float FPS_CALC_INTERVAL = 1.0f;

	float fps;
	float timer = 0;


	// Update is called once per frame
	void Update () {

		++counter;
		timer += Time.deltaTime;

		if (timer >= FPS_CALC_INTERVAL) {
			fps = counter/timer;
			GetComponent<Text> ().text = fps.ToString ("00.00");
			timer = 0;
			counter = 0;
		}

	}
}
