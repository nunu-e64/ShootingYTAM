using UnityEngine;
using System.Collections;

/// <summary>
/// 各色の敵の変更をすべてのウェーブに反映させるためのダミーオブジェクト。
/// ・現在Unity5.1ではプレハブの中にプレハブを置くと子プレハブのリンクが切れてしまう仕様のため、自前で疑似リンクを作る
/// ・Stage、Wave作成時は、Enemyを直接配置する代わりにEnemyDammyを配置する
///	・ゲーム開始時（インスタンス化）に自動的に指定したエネミーに置き換わる
/// </summary>
public class EnemyDammy : MonoBehaviour {

	public GameObject enemy;
	public bool canShot = true;


	void Start () {

		GameObject go = Instantiate (enemy, transform.position, transform.rotation) as GameObject;
		go.transform.SetParent(transform.parent);

		enemy.GetComponent<Enemy> ().canShot = canShot;

		Destroy (gameObject);

	}
	
}
