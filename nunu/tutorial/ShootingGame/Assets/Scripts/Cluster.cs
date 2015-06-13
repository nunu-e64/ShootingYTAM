using UnityEngine;
using System.Collections;

/// <summary>
/// Waveの出現を管理
/// ・Emitterでインスタンス化
/// ・ClusterとWaveについてはEmitter.cを参照
/// </summary>
public class Cluster : MonoBehaviour {
	
	[System.Serializable]
	private class WaveData {		//TIPS: classであれば[System.Serializable]を指定すればInspectorから編集できる。structでは不可。
		public Wave wave;			//Enemyの1群
		public float nextAppearTime;	//このWaveが出現してから次のWaveが出現するまでの時間[s] Cluster最後のWaveの時は次のClusterに移行するまでの時間[s]
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
			Debug.Log ("<color=red>Destroy Cluster</color> waves.Length=0");
		}
	}
	
	void Update () {
		timer += Time.deltaTime;

		if (currentWaveIndex == -1 || timer > waves[currentWaveIndex].nextAppearTime) {

			timer = 0;
			++currentWaveIndex;

			if (currentWaveIndex < waves.Length) {
				Wave currentWave = Instantiate (waves[currentWaveIndex].wave);
				Debug.Log ("<color=green>" + gameObject + "Create:</color>" + currentWave);

			} else {
				Destroy (gameObject);
			}

		}		
	}

}
