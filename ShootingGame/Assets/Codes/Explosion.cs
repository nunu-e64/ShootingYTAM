using UnityEngine;
using System.Collections;

/// <summary>
/// PlayerとEnemyの爆破演出
/// </summary>
public class Explosion : MonoBehaviour {

	public void SetTimer (float stayTime) {
		GetComponent<Animator> ().SetFloat ("timer", stayTime);
	}

	void Update () {
		GetComponent<Animator> ().SetFloat ("timer", GetComponent<Animator> ().GetFloat ("timer") - Time.deltaTime);
	}

	void OnAnimationFinish() {
		Destroy(gameObject);
	}

}
