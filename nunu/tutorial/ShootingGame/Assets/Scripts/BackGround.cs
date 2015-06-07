using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {

	public float speed = 0.1f;

	void Start() {
		//画面右上のワールド座標をビューポートから取得
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		Vector2 scale = max * 2;	//ワールド座標は画面中心が原点になっているので背景画像のスケールは2倍して求める
		transform.localScale = scale;
	}

	// Update is called once per frame
	void Update () {
	
		float y = Mathf.Repeat(Time.time * speed , 1);
		Vector2 offset = new Vector2(0, y);

		GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

	}

	void OnDestroy () {
		GetComponent<Renderer> ().sharedMaterial.SetTextureOffset ("_MainTex", new Vector2 (0, 0));
	}
}
