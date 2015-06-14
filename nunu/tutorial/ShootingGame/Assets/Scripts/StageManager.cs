using UnityEngine;
using System.Collections;

/// <summary>
/// 敵の出現管理クラス
/// ・管理単位･･･Stage > Wave > Enemy
/// ・WaveはEnemyをいくつか含み、StageはWaveをいくつか含む
/// ・WaveとStageは事前作成しPrefab化しておく
/// ・StageはStageManagerによって順次出現させる。Waveの出現はStageが管理する。
/// </summary>
public class StageManager : MonoBehaviour {

	[SerializeField]
	private Manager manager;

	[SerializeField]
	private GameObject bulletPool;

	[SerializeField]
	private Stage[] stages;		//敵の集団=waveの配列	inspectorから事前にセット

	[SerializeField]
	private bool isLoop;	//すべてのStageを出現させたあとリスタートするかのフラグ

	private Stage currentStage;
	private int currentStageIndex;		//現在何番目のwaveを表示しているか


	void Start(){
		currentStageIndex = -1;

		if (stages.Length == 0) {	//stageが未セットの場合は終了
			Destroy (gameObject);
			Debug.Log ("<color=red>Destroy StageManager</color> stages.Length=0");
		}
	}

	public void Init () {	//タイトル表示の際に呼び出す初期化処理
		currentStageIndex = -1;
		currentStage = null;
		foreach (Transform child in gameObject.transform) {	//すべてのStage,Wave,Enemyを破棄
			Destroy (child.gameObject);
		}
		if (bulletPool != null) {
			foreach (Transform child in bulletPool.transform) {	//すべてのStage,Wave,Enemyを破棄
				Destroy (child.gameObject);
			}
		}
	}

	void Update () {

		if (manager.IsPlaying ()) {

			if (currentStage == null && currentStageIndex < stages.Length) {

				++currentStageIndex;

				//すべてのStageの出現が完了したとき→再度最初から出現or停止
				if (currentStageIndex == stages.Length) {
					if (isLoop) {
						currentStageIndex = 0;
						Debug.Log ("<color=yellow>Reset StageIndex </color> :" + currentStageIndex);
					} else {
						Debug.Log ("<color=yellow>Finish Emit</color> :" + currentStageIndex);
						return;
					}
				}

				//新しいStageを作成しEmmiterの子要素とする
				currentStage = Instantiate (stages[currentStageIndex]);
				currentStage.transform.parent = transform;
				Debug.Log ("<color=green>" + gameObject + "Create:</color>" + currentStage);

			}
		}

	}
	
}
