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

	[SerializeField]
	private Manager manager;

	[SerializeField]
	private GameObject bulletPool;

	[SerializeField]
	private Cluster[] clusters;		//敵の集団=waveの配列	inspectorから事前にセット

	[SerializeField]
	private bool isLoop;	//すべてのclusterを出現させたあとリスタートするかのフラグ

	private Cluster currentCluster;
	private int currentClusterIndex;		//現在何番目のwaveを表示しているか


	void Start(){
		currentClusterIndex = -1;

		if (clusters.Length == 0) {	//clusterが未セットの場合は終了
			Destroy (gameObject);
			Debug.Log ("<color=red>Destroy Emitter</color> clusters.Length=0");
		}
	}

	public void Init () {	//タイトル表示の際に呼び出す初期化処理
		currentClusterIndex = -1;
		currentCluster = null;
		foreach (Transform child in gameObject.transform) {	//すべてのCluster,Wave,Enemyを破棄
			Destroy (child.gameObject);
		}
		if (bulletPool != null) {
			foreach (Transform child in bulletPool.transform) {	//すべてのCluster,Wave,Enemyを破棄
				Destroy (child.gameObject);
			}
		}
	}

	void Update () {

		if (manager.IsPlaying ()) {

			if (currentCluster == null && currentClusterIndex < clusters.Length) {

				++currentClusterIndex;

				//すべてのclusterの出現が完了したとき→再度最初から出現or停止
				if (currentClusterIndex == clusters.Length) {
					if (isLoop) {
						currentClusterIndex = 0;
						Debug.Log ("<color=yellow>Reset ClusterIndex </color> :" + currentClusterIndex);
					} else {
						Debug.Log ("<color=yellow>Finish Emit</color> :" + currentClusterIndex);
						return;
					}
				}

				//新しいClusterを作成しEmmiterの子要素とする
				currentCluster = Instantiate (clusters[currentClusterIndex]);
				currentCluster.transform.parent = transform;
				Debug.Log ("<color=green>" + gameObject + "Create:</color>" + currentCluster);

			}
		}

	}
	
}
