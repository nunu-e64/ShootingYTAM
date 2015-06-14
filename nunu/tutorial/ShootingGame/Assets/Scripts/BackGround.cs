using UnityEngine;
using System.Collections;

/// <summary>
/// 背景画像スクロールクラス
/// ・背景は3枚のテクスチャそれぞれにこのスクリプトをアタッチし、速度差をつけてoffsetを変えることで動いているように見せかけている
/// </summary>
public class BackGround : MonoBehaviour {
	
	public float speed = 0.1f;

	void Start() {
		//画面右上のワールド座標をビューポートから取得し、背景画像サイズを画面に合わせて伸縮させる
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));		//画面右上のワールド座標取得
		Vector2 scale = max * 2;												//TIPS: ワールド座標は画面中心が原点になっているので背景画像のスケールは2倍して求める
		transform.localScale = scale;
	}

	void Update () {	//移動処理
	
		float y = Mathf.Repeat(Time.time * speed , 1);
		Vector2 offset = new Vector2(0, y);

		GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

	}

	void OnDestroy () {		//ゲーム終了時にoffsetの変更を保持しないように初期化する
		GetComponent<Renderer> ().sharedMaterial.SetTextureOffset ("_MainTex", new Vector2 (0, 0));
	}
}
