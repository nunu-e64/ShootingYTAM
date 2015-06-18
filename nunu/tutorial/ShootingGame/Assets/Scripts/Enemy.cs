using UnityEngine;
using System.Collections;

/// <summary>
/// 敵単体クラス
/// </summary>
public class Enemy : Spaceship {

	[HeaderAttribute ("EnemyStatus")]
	public int point = 100;		//倒した時に手に入るスコア
	public int hp = 1;

	private float shotTimer;	//弾発射間隔を制御する経過時間記録変数
	private Animator animator;

	[HeaderAttribute ("BulletStatus")]
	public bool shotable = true;	//弾を発射するか
	public GameObject bullet;		//弾のプレハブ
	public float shotDelay;			//弾の発射間隔[s]

	void Start () {

		//速度は最初に設定し以降一定速度で移動
		GetComponent<Rigidbody2D> ().velocity = transform.up.normalized * -1 * speed;

		animator = GetComponent<Animator> ();
	
	}

	void Update(){

		if (!shotable) {
			return ;
		}
			
		shotTimer+=Time.deltaTime;

		if (shotTimer > shotDelay) {

			//画面内にあるときすべての子要素(shotPosition=砲台)から弾を発射////
			Vector3 positionInView = Camera.main.WorldToViewportPoint (transform.position);

			if (positionInView.x > 0 && positionInView.x < 1 && positionInView.y > 0 && positionInView.y < 1) {
				foreach (Transform shotPosition in transform) { 
					ObjectPool.Instance.GetGameObject(bullet, shotPosition.position, shotPosition.rotation);
				}
				shotTimer = 0;
			}
			///////////////////////////////////////////////////////////////////

        }
    
    }
	
	void OnTriggerEnter2D(Collider2D c) {

		//レイヤーに応じて処理を分岐
		string layerName = LayerMask.LayerToName(c.gameObject.layer);

		if (layerName == "Bullet(Player)") {

			//弾の威力に応じてHPを減らす。弾の威力はEnemyにHP分だけ減じる
			Bullet bullet = c.transform.parent.GetComponent<Bullet>();
			int oldHp = hp;
			hp -= bullet.power;
			bullet.power -= oldHp;

			if (bullet.power <= 0) {
				c.gameObject.SetActive (false);
			}

			if (hp <= 0) {	//HPがなくなればスコア加算して死亡
				FindObjectOfType<ScoreManager>().AddPoint(point);
				Explosion();
				Destroy(gameObject);
			} else {
				animator.SetTrigger("Damage");
			}
		}
	}
}
