using UnityEngine;
using System.Collections;

//必須コンポーネント（この記述がある限り該当コンポーネントを削除できない）
[RequireComponent(typeof(Rigidbody2D))]


public class Spaceship : MonoBehaviour {

    public float speed;
    public float shotDelay;
    public GameObject bullet;
	public bool shotable = true;

	public GameObject explosion;

	public Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
	}

    //弾の発射
    public void Shot(Transform origin)
    {
        Instantiate(bullet, origin.position, origin.rotation);
    }

	//爆発の生成
	public void Explosion() {
		Instantiate(explosion, transform.position, transform.rotation);
	}

	public Animator GetAnimator(){
		return animator;
	}

}
