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


	private Dictionary<int, GameObject> pooledObjects = new Dictionary<int, GameObject> ();		//プールしているオブジェクト。Key:インスタンスID、Value:GameObject
	private Dictionary<int, Stack<int>> unActiveObjects = new Dictionary<int, Stack<int>> ();	//非アクティブ＝未使用のオブジェクト。Key:レイヤー(オブジェクトの種類)、Value:インスタンスIDのStack


	//未使用のゲームオブジェクトをpooledGameObjectsから取得する。必要であれば新たに生成する
	public GameObject GetGameObject (GameObject prefab, Vector2 position, Quaternion rotation) {
		
		int key = prefab.layer;

		//keyが存在しなければ作成する
		if (unActiveObjects.ContainsKey (key) == false) {
			unActiveObjects.Add (key, new Stack<int> ());
		}

		//未使用オブジェクトがあればそれを使う
		if (unActiveObjects[key].Count > 0) {
			GameObject go = pooledObjects[unActiveObjects[key].Pop()];
			go.transform.position = position;
			go.transform.rotation = rotation;
			go.SetActive (true);
			foreach (Transform child in go.transform) {		//PlayerBulletは子もアクティブにしなくてはいけない
				child.gameObject.SetActive (true);				
			}
			return go;
		}

		//未使用オブジェクトが存在しなかったので新規作成してDictionalyに追加
		GameObject newGo = (GameObject) Instantiate (prefab, position, rotation);
		newGo.transform.parent = transform;
		pooledObjects.Add(newGo.GetInstanceID(), newGo);

		return newGo;
	}

	// ゲームオブジェクトを非アクティブにする。こうすることで再利用可能状態にする
	public void ReleaseGameObject (GameObject go) {
		go.SetActive (false);

		if (unActiveObjects.ContainsKey (go.layer)) {
			unActiveObjects[go.layer].Push (go.GetInstanceID ());
		} else {
			Destroy (go);
		}
	}

	//一括非アクティブ化
	public void ReleaseAllGameObject () {

		foreach (GameObject go in pooledObjects.Values) {
			ReleaseGameObject (go);
		}

	}
}