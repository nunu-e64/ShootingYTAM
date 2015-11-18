using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

	void Update () {

		//子要素＝EnemyがなくなったときにはWave自身も破棄
		if (transform.childCount == 0) {
			Destroy (gameObject);
		}
	
	}
}
