using UnityEngine;
using System.Collections;

/// <summary>
/// Waveの出現を管理
/// ・StageManagerでインスタンス化
/// ・StageとWaveについてはStageManager.csを参照
/// </summary>
public class Stage : MonoBehaviour {
	
	[System.Serializable]
	private class WaveData {		//TIPS: classであれば[System.Serializable]を指定すればInspectorから編集できる。structでは不可。
		public Wave wave;			//Enemyの1群
		public float nextAppearTime;	//このWaveが出現してから次のWaveが出現するまでの時間[s] Stage最後のWaveの時は次のStageに移行するまでの時間[s]
	}

	[SerializeField]	//TIPS: [SerializeField]を指定するとprivate変数をInspectorで編集できるようになる。
	private WaveData[] waves;

	private float timer;	//waveの出現タイミングを管理するためのタイマー。wave出現ごとにリセット。	
	private int currentWaveIndex;

	void Start () {
		timer = 0;
		currentWaveIndex = -1;
		
		if (waves.Length == 0) {
			Destroy (gameObject);
			Debug.Log ("<color=red>Destroy Stage</color> waves.Length=0");
		}
	}
	
	void Update () {
		timer += Time.deltaTime;

		if (currentWaveIndex == -1 || timer > waves[currentWaveIndex].nextAppearTime) {

			timer = 0;
			++currentWaveIndex;

			if (currentWaveIndex < waves.Length) {
				Wave currentWave = Instantiate (waves[currentWaveIndex].wave);
				currentWave.transform.parent = transform.parent;
				Debug.Log ("<color=green>CreateWave:</color>" + currentWave);

			} else {
				Destroy (gameObject);
			}

		}		
	}

}
