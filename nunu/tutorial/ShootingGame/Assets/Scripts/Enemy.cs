using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    Spaceship spaceship;


    IEnumerator Start () {
			
        spaceship = GetComponent<Spaceship>();


		if (!spaceship.canShot) {
			yield break;
		}
			
		while (true) {

			//子要素を全て取得
			for (int i = 0; i < transform.childCount; i++) {
				Transform shotPosition = transform.GetChild(i);
				spaceship.Shot(shotPosition);
			}

			//shotDelay秒待つ
			yield return new WaitForSeconds(spaceship.shotDelay);
        }
    
    }

	void Update() {
		//移動
		Move(transform.up.normalized * -1);
	}


	//移動
	public void Move(Vector2 direction) {
		GetComponent<Rigidbody2D>().velocity = direction * spaceship.speed;
	}

	void OnTriggerEnter2D(Collider2D c) {

		//レイヤーに応じて処理を分岐
		string layerName = LayerMask.LayerToName(c.gameObject.layer);

		if (layerName == "Bullet(Player)") {
			Destroy(c.gameObject);
			spaceship.Explosion();
			Destroy(gameObject);
		}

	}
}
