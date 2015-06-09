using UnityEngine;
using System.Collections;

/// <summary>
/// 敵の出現管理クラス
/// ・WaveというEnemyをいくつか含む集団単位で管理する
/// ・Waveは事前作成してPrefab化しEmitter.waves[]に必要なだけセットしておく
/// ・一つのWaveが終わるごとに、順次次のWaveを出現させ、すべてのWaveが出現したら再度一つ目のWaveから出現する
/// </summary>
public class Emitter : MonoBehaviour {

	public GameObject[] waves;		//敵の集団=waveの配列	inspectorから事前にセット
	private int currentWave;		//現在何番目のwaveを表示しているか
	private Manager manager;

	// Use this for initialization
	IEnumerator Start () {

		if (waves.Length == 0) {	//Waveが存在しなければコルーチンを終了する
			yield break;
		}

		manager = FindObjectOfType<Manager>();


		//Wave順次出現ルーチン
		while (true) {
			while (!manager.IsPlaying()) {		//ゲームプレイ時以外は待機
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

			//currentWaveをインクリメントして次のWaveに移行
			if (waves.Length <= ++currentWave) {
				currentWave = 0;
				Debug.Log("currentWave Reset");
			}
		}

	}
	
}
