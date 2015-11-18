using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeCounter : MonoBehaviour {

	float timer = 0;	//時間計測カウンタ
	bool enable = false;

	void Start () {
		SetEnable (false);
	}

	public void SetEnable (bool _enable) {
		enable = _enable;
		if (!enable) {
			timer = 0;
			GetComponent<Text> ().text = "";
		}
	}

	void Update () {
		if (enable) {
			timer += Time.deltaTime;

			GetComponent<Text> ().text = timer.ToString ("0.0");
		}
	}
}