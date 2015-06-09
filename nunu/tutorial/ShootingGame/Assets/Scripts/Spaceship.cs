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

	public float speed;				//機体の移動速度[Unity/s]
    public float shotDelay;			//弾の発射間隔[s]
    public GameObject bullet;
	public bool shotable = true;

	public GameObject explosion;
	public Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
	}

    public void Shot(Transform origin) { 
        Instantiate(bullet, origin.position, origin.rotation);
    }

	public void Explosion() {
		Instantiate(explosion, transform.position, transform.rotation);
	}

	public Animator GetAnimator(){
		return animator;
	}

}
