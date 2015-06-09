using UnityEngine;
using System.Collections;

/// <summary>
/// 画面外に出た敵や弾の削除
/// </summary>
public class DestroyArea : MonoBehaviour {

	void OnTriggerExit2D (Collider2D c) {	//TIPS: 判定の有無はUnityのEdit->ProjectSetting->Physics2Dで設定
		Destroy(c.gameObject);
	}

}
