using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {

	public GameObject[] waves;

	private int currentWave;

	// Use this for initialization
	IEnumerator Start () {

		if (waves.Length == 0) {	//Waveが存在しなければコルーチンを終了する
			yield break;
		}

		while (true) {
			
			//新しいwaveを作成しEmmiterの子要素とする
			GameObject wave = (GameObject)Instantiate(waves[currentWave], transform.position, Quaternion.identity);
			wave.transform.parent = transform;
		
			//Waveの子要素のEmenyがすべて削除されるまで待機
			while (wave.transform.childCount !=0) {
				yield return new WaitForEndOfFrame();
			}

			Destroy(wave);

			if (waves.Length <= ++currentWave) {
				currentWave = 0;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
