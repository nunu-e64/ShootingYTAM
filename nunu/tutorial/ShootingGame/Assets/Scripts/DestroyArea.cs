using UnityEngine;
using System.Collections;

/// <summary>
/// 画面外に出た敵や弾の削除を行う
/// </summary>
public class DestroyArea : MonoBehaviour {

	void Start () {
		//Scaleを画面に合わせる
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));		//画面右上のワールド座標取得
		Vector2 scale = max * 2;											
		transform.localScale = scale;
	}

	void OnTriggerExit2D (Collider2D c) {	//TIPS: 判定の有無はUnityのEdit->ProjectSetting->Physics2Dで設定

		//弾のときはプールから解放する
		if (c.CompareTag("Bullet")) {
			ObjectPool.Instance.ReleaseGameObject (c.gameObject);
		} else {
			Destroy(c.gameObject);
		}
	}

}
