using UnityEngine;
using System.Collections;

//TIPS: 必須コンポーネント（この記述がある限り該当コンポーネントを削除できない）
[RequireComponent(typeof(Rigidbody2D))]


/// <summary>
/// PlayerとEnemyが持つ機体汎用クラス	
/// ・移動と死亡時の爆発描画
/// ・移動速度など変数の管理
/// </summary>
public class Spaceship : MonoBehaviour {	//TODO: 継承で代替すべき

	[HeaderAttribute ("ShipStatus")]
	public float speed;				//機体の移動速度[Unity/s]

	[HeaderAttribute ("BulletStatus")]
	public bool shotable = true;	//弾を発射するか
	public GameObject bullet;		//弾のプレハブ
	public float shotDelay;			//弾の発射間隔[s]

	[HeaderAttribute ("OtherRef")]
	public GameObject explosion;

	private GameObject bulletPool;
	private Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
		if ((bulletPool = GameObject.Find ("BulletPool")) == null) {
			Debug.LogError("Not Found GameObject\"BulletPool\"");
		}
	}

    public void Shot(Transform origin) { 
        GameObject newBullet = Instantiate(bullet, origin.position, origin.rotation) as GameObject;
		if (bulletPool != null) newBullet.transform.parent = bulletPool.transform;
    }

	public void Explosion() {
		Instantiate(explosion, transform.position, transform.rotation);
	}

	public Animator GetAnimator(){
		return animator;
	}

}
