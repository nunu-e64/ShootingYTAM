using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {

	public GameObject[] waves;

	private int currentWave;

	private Manager manager;

	// Use this for initialization
	IEnumerator Start () {

		if (waves.Length == 0) {	//Waveが存在しなければコルーチンを終了する
			yield break;
		}

		manager = FindObjectOfType<Manager>();

		while (true) {
			while (!manager.IsPlaying()) {
				yield return new WaitForEndOfFrame();
			}

			//新しいwaveを作成しEmmiterの子要素とする
			GameObject wave = (GameObject)Instantiate(waves[currentWave], waves[currentWave].transform.position, Quaternion.identity);
			wave.transform.parent = transform;
		
			//Waveの子要素のEmenyがすべて削除されるまで待機
			while (wave.transform.childCount !=0) {
				yield return new WaitForEndOfFrame();
			}

			Destroy(wave);

			if (waves.Length <= ++currentWave) {
				currentWave = 0;
				Debug.Log("currentWave Reset");
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
