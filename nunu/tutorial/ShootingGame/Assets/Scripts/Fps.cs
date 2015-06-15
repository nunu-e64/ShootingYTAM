using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fps : MonoBehaviour {

	const float FPS_CALC_INTERVAL = 1.0f;	//何秒単位でFPS表示を更新するか

	float counter = 0;	//フレームカウンタ
	float timer = 0;	//実時間カウンタ


	void Update () {

		++counter;
		timer += Time.deltaTime;

		if (timer >= FPS_CALC_INTERVAL) {
			float fps = counter/timer;
			GetComponent<Text> ().text = fps.ToString ("00.00");
			timer = 0;
			counter = 0;
		}

	}
}
