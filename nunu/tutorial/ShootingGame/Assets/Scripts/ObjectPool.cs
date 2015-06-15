using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 何度も生成していたゲームオブジェクトを、使いまわすことでコストの高いIntanciateの利用を避けるための管理クラス
/// ・オブジェクトの実体はDictionalyに保管する
/// </summary>
public class ObjectPool : MonoBehaviour {

	// シングルトン////////////////////////////////////////
	private static ObjectPool instance;

	public static ObjectPool Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<ObjectPool> ();		// シーン上から取得する

				if (instance == null) {
					instance = new GameObject ("ObjectPool").AddComponent<ObjectPool> ();	// ゲームオブジェクトを作成しObjectPoolコンポーネントを追加する
				}
			}
			return instance;
		}
	}
	///////////////////////////////////////////////////////


	//GameObjectのDictionaly。プレハブのインスタンスIDをkeyとする
	private Dictionary<int, List<GameObject>> pooledGameObjects = new Dictionary<int, List<GameObject>> ();


	// ゲームオブジェクトをpooledGameObjectsから取得する。必要であれば新たに生成する
	public GameObject GetGameObject (GameObject prefab, Vector2 position, Quaternion rotation) {
		
		int key = prefab.GetInstanceID ();

		// Dictionaryにkeyが存在しなければ作成する
		if (pooledGameObjects.ContainsKey (key) == false) {
			pooledGameObjects.Add (key, new List<GameObject> ());
		}

		List<GameObject> gameObjects = pooledGameObjects[key];
		GameObject go = null;

		for (int i = 0; i < gameObjects.Count; i++) {

			go = gameObjects[i];

			// 現在非アクティブなオブジェクトがリストにあればそれを位置設定して利用。なければ新規作成
			if (go.activeInHierarchy == false) {
				go.transform.position = position;
				go.transform.rotation = rotation;
				go.SetActive (true);
				return go;
			}
		}

		//未使用オブジェクトが存在しなかったので新規作成
		go = (GameObject) Instantiate (prefab, position, rotation);
		go.transform.parent = transform;
		gameObjects.Add (go);

		return go;
	}

	// ゲームオブジェクトを非アクティブにする。こうすることで再利用可能状態にする
	public void ReleaseGameObject (GameObject go) {
		go.SetActive (false);
	}

	//一括非アクティブ化
	public void ReleaseAllGameObject () {

		foreach (List<GameObject> list in pooledGameObjects.Values) {
			foreach (GameObject go in list) {
				go.SetActive (false);
			}
		}

	}
}