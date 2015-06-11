using UnityEngine;
using System.Collections;

/// <summary>
/// 敵の出現管理クラス
/// ・管理単位･･･Cluster > Wave > Enemy
/// ・WaveはEnemyをいくつか含み、ClusterはWaveをいくつか含む
/// ・WaveとClusterは事前作成しPrefab化しておく
/// ・ClusterはEmitterによって順次出現させる。WaveはClusterが管理する。
/// </summary>
public class Emitter : MonoBehaviour {

	public Cluster[] clusters;		//敵の集団=waveの配列	inspectorから事前にセット

	private Cluster currentCluster;
	private int currentClusterIndex;		//現在何番目のwaveを表示しているか
	private Manager manager;


	void Start(){
		manager = FindObjectOfType<Manager>();
		currentClusterIndex = -1;

		if (clusters.Length == 0) {
			Destroy (gameObject);
		}
	}


	void Update () {

		if (manager.IsPlaying ()) {

			if (currentCluster == null) {
				//currentClusterIndexをインクリメント
				if (clusters.Length <= ++currentClusterIndex) {
					currentClusterIndex = 0;
					Debug.Log ("<color=yellow>Reset ClusterIndex : </color>" + currentClusterIndex);
				}

				//新しいClusterを作成しEmmiterの子要素とする
				currentCluster = Instantiate (clusters[currentClusterIndex]);
				currentCluster.transform.parent = transform;
				Debug.Log ("<color=yellow>Create : </color>" + currentCluster);

			}
		}

	}
	
}
