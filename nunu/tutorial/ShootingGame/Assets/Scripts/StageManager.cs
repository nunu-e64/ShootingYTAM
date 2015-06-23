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

	[Space(15)]
	public Manager manager;

	[Space (15)]
	public float breakTimeBetweenStages = 2.0f;
	public bool isLoop;	//すべてのStageを出現させたあとリスタートするかのフラグ
	public float speedRateUpPace = 0.2f;		//SpeedRateの上昇量

	public Stage[] stages;		//敵の集団=waveの配列	inspectorから事前にセット

	private Stage currentStage;
	private int currentStageIndex;			//現在何番目のwaveを表示しているか

	private float speedRate = 1.0f;			//段々難化させるためにステージがループするごとに敵の速度を上げる.その際のスピードの倍率 
	private float stageChangeTimer;

	void Start(){
		currentStageIndex = -1;
		stageChangeTimer = 0;

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

		ObjectPool.Instance.ReleaseAllGameObject ();	//プール内のすべてのオブジェクトを解放（実際は非アクティブに切り替え）

		speedRate = 1.0f;
	}

	void Update () {

		if (manager.IsPlaying ()) {
			
			if (currentStage == null && currentStageIndex < stages.Length) {

				stageChangeTimer += Time.deltaTime;

				if (stageChangeTimer >= breakTimeBetweenStages) {
					stageChangeTimer = 0;
					++currentStageIndex;

					//すべてのStageの出現が完了したとき→再度最初から出現or停止
					if (currentStageIndex == stages.Length) {
						if (isLoop) {
							currentStageIndex = 0;
							speedRate += speedRateUpPace;
							Debug.Log ("<color=yellow>Reset StageIndex </color> :" + currentStageIndex + "\nspeedRate : " + speedRate);
						} else {
							Debug.Log ("<color=yellow>Finish Emit</color> :" + currentStageIndex);
							return;
						}
					}

					//新しいStageを作成しEmmiterの子要素とする
					currentStage = Instantiate (stages[currentStageIndex]);
					currentStage.transform.parent = transform;
					currentStage.SetEnemySpeedRate (speedRate);
					Debug.Log ("<color=green>CreateStage:</color>" + currentStage);
				}

			}
		}

	}
	
}
