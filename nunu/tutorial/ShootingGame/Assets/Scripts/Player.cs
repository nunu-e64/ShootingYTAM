using UnityEngine;
using System.Collections;

/// <summary>
/// 自機クラス
/// </summary>
public class Player : Spaceship {
	
	public float shotNum = 1;			//砲台セット数(砲台1個＝弾2発(15/06/09現在))	//Animatorから変更するためにintではなくfloatでないといけない
	public float touchPosGapY = 1.0f;	//移動の際に指で機体が隠れないようにタップした位置からずらす値

	private Animator animator;
	
	IEnumerator Start () {		
		AudioSource shotAudio = GetComponent<AudioSource> ();
		FindObjectOfType<GaugeManager>().SetPlayer(this);
		animator = GetComponent<Animator>();

		//弾発射ループ
        while (true){

			if (shotable) {

				//子要素を全て取得して弾を発射
				for (int i = 0; i < transform.childCount && i < (int)shotNum; i++) {
					Transform shotPosition = transform.GetChild(i);
					Shot(shotPosition);
				}

				//弾発射SE再生
				if (shotNum > 0) {
					if (shotAudio.isActiveAndEnabled) shotAudio.Play ();
					else Debug.LogWarning ("OK");
				}
			}
			yield return new WaitForSeconds(shotDelay);
        }

	}

	void Update () {

		//入力に基づいて目標座標を求め移動
		Vector2 targetWorldPosition;
		if (Input.touchCount > 0) {		//タッチ入力
			targetWorldPosition = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			targetWorldPosition.y += touchPosGapY;

		} else if (Input.GetMouseButton(0)) {	//マウス入力
			targetWorldPosition = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
			targetWorldPosition.y += touchPosGapY;

		} else {	//キー入力
			targetWorldPosition = new Vector2 (transform.position.x, transform.position.y) + new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")) * speed * Time.deltaTime / 5; 
		}

		Move (targetWorldPosition);

		//スペシャルアタック（仮）
		if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) {
			InvokeSpecialAttack ();
		}
	}


	//移動＋移動制限
	void Move(Vector2 targetPos) {
		
		//画面左下と右上のワールド座標をカメラのビューポート（0~1）から変換して取得
		Vector2 min = (Vector2) Camera.main.ViewportToWorldPoint (new Vector2 (0, 0)) +new Vector2 (0.3f, 0.3f);
		Vector2 max = (Vector2) Camera.main.ViewportToWorldPoint (new Vector2 (1, 1)) + new Vector2 (-0.3f, -0.3f);
		
		Vector2 pos = transform.position;
		Vector2 header = (targetPos - (Vector2)transform.position);
		float moveDistance = speed * Time.deltaTime;


		//目標座標が近ければ目標座標に自機座標を一致させ, 目標座標が遠ければSpeed*Time.deltaTimeだけ移動
		if (header.sqrMagnitude < moveDistance * moveDistance) {
			pos = targetPos;
		} else {
			Vector2 direction = header.normalized;
			pos += direction * moveDistance;
		}

		pos.x = Mathf.Clamp(pos.x, min.x, max.x);
		pos.y = Mathf.Clamp(pos.y, min.y, max.y);

		transform.position = pos;
	}

	//被弾判定
	void OnTriggerEnter2D(Collider2D c) {

		//レイヤーに応じて処理を分岐
		string layerName = LayerMask.LayerToName(c.gameObject.layer);

		if (layerName == "Bullet(Enemy)") {
			ObjectPool.Instance.ReleaseGameObject(c.gameObject);
		}

		if (layerName == "Bullet(Enemy)" || layerName == "Enemy") {
			OnDead ();
		}

	}

	//死亡処理		
	public void OnDead () {	
		Explosion ();
		Destroy (gameObject);

		FindObjectOfType<Manager> ().GameOver ();
	}

	//特殊攻撃
	public void InvokeSpecialAttack(){
		animator.SetTrigger("Special");
	}
}
