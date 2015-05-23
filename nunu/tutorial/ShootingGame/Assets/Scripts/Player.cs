using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Spaceship spaceship;
	private Joystick joystick;

	// Use this for initialization
	IEnumerator Start () {  //Updateに書くと他の処理に影響を及ぼす恐れがあるためコルーチンを利用

        spaceship = GetComponent<Spaceship> ();
		joystick = FindObjectOfType<Joystick>();

        while (true){
            spaceship.Shot(transform);

			GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(spaceship.shotDelay);
        }

	}


	// Update is called once per frame
	void Update () {

		//キーボード入力
		//float x = Input.GetAxisRaw("Horizontal");
		//float y = Input.GetAxisRaw("Vertical");

		//仮想ジョイステック入力
		//float x = joystick.position.x;
		//float y = joystick.position.y;

		Vector2 targetWorldPosition;
		if (Input.touchCount > 0) {	//タッチ入力
			targetWorldPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Move(targetWorldPosition);
		} else if (Input.GetMouseButton(0)) {	//マウス入力
			targetWorldPosition = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
			Move(targetWorldPosition);
		}

	}


	//移動＋移動制限
	void Move(Vector2 targetPos) {
		
		//画面左下と右上のワールド座標をカメラのビューポート（0~1）から変換して取得
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		
		Vector2 pos = transform.position;
		Vector2 header = (targetPos - (Vector2)transform.position);
		float moveDistance = spaceship.speed * Time.deltaTime;


		//目標座標が近ければ目標座標に自機座標を一致させ, 目標座標が遠ければSpeed分だけ移動
		if (header.sqrMagnitude < moveDistance * moveDistance) {	//平方根計算、二乗計算は負荷が高いため避ける
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
			Destroy(c.gameObject);
		}

		if (layerName == "Bullet(Enemy)" || layerName == "Enemy") {
			spaceship.Explosion();
			Destroy(gameObject);

			FindObjectOfType<Manager>().GameOver();
		}


	}
}
