﻿using UnityEngine;
using System.Collections;

/// <summary>
/// 敵単体クラス
/// </summary>
public class Enemy : MonoBehaviour {

	public int point = 100;		//倒した時に手に入るスコア
	public int hp = 1;

    Spaceship spaceship;


    IEnumerator Start () {

		spaceship = GetComponent<Spaceship> ();
		GetComponent<Rigidbody2D> ().velocity = transform.up.normalized * -1 * spaceship.speed;

		if (!spaceship.shotable) {
			yield break;
		}
			
		//弾発射ルーチン
		while (true) {

			//子要素(砲台)を全て取得して弾を発射
			for (int i = 0; i < transform.childCount; i++) {
				Transform shotPosition = transform.GetChild(i);
				spaceship.Shot(shotPosition);
			}

			yield return new WaitForSeconds(spaceship.shotDelay);
        }
    
    }

	void OnTriggerEnter2D(Collider2D c) {

		//レイヤーに応じて処理を分岐
		string layerName = LayerMask.LayerToName(c.gameObject.layer);

		if (layerName == "Bullet(Player)") {

			//弾の威力に応じてHPを減らし弾を削除
			Bullet bullet = c.transform.parent.GetComponent<Bullet>();
			hp -= bullet.power;
			Destroy(c.gameObject);

			if (hp <= 0) {	//HPがなくなればスコア加算して死亡
				FindObjectOfType<ScoreManager>().AddPoint(point);
				spaceship.Explosion();
				Destroy(gameObject);
			} else {
				spaceship.GetAnimator().SetTrigger("Damage");
				//Debug.Log("Damaged");		//DEBUG
			}
		}
	}
}
