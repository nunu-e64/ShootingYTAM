using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Spaceship spaceship;

	// Use this for initialization
	IEnumerator Start () {  //Updateに書くと他の処理に影響を及ぼす恐れがあるためコルーチンを利用

        spaceship = GetComponent<Spaceship> ();

        while (true){
            spaceship.Shot(transform);

			GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(spaceship.shotDelay);
        }

	}
	
	// Update is called once per frame
	void Update () {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2 (x, y).normalized;
        
		//spaceship.Move(direction);

		//移動
		Move(direction);
	}


	//移動＋移動制限
	void Move(Vector2 direction) {
		//画面左下と右上のワールド座標をカメラのビューポート（0~1）から変換して取得
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		Vector2 pos = transform.position;

		pos += direction * spaceship.speed * Time.deltaTime;

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
		}


	}
}
