using UnityEngine;
using System.Collections;

/// <summary>
/// Waveの出現を管理
/// ・Emitterでインスタンス化
/// </summary>
public class Cluster : MonoBehaviour {

	[System.Serializable]
	private class WaveData {
		public GameObject wave;
		public float appearTime;
	}
	
	[SerializeField] 
	private WaveData[] waves;

	private GameObject currentWave;
	private int currentWaveIndex;

	
	void Start () {
		currentWaveIndex = 0;
		
		if (waves.Length == 0) {
			Destroy (gameObject);
			Debug.Log ("<color=green>Destroy Cluster</color>");
		}
	}
	
	void Update () {

		if (currentWave == null) {

			currentWave = (GameObject) Instantiate (waves[currentWaveIndex].wave, waves[currentWaveIndex].wave.transform.position, Quaternion.identity);
			currentWave.transform.parent = transform;
			Debug.Log ("Create:" + currentWave);

		} else {

			if (currentWave.transform.childCount == 0) {

				Destroy (currentWave);

				if (waves.Length <= ++currentWaveIndex) {
					Destroy (gameObject);
					Debug.Log ("<color=green>Destroy Cluster</color>");
				}
			}

		}

		
	}
}
