using UnityEngine;
using System.Collections;

/// <summary>
/// 弾単体クラス
/// ・まっすぐ飛んでいく
/// </summary>
public class Bullet : MonoBehaviour {
	public float speed = 10;
	public float lifeTime = 5;

	public int power = 1;

	private float livingTimer;

	void OnEnable () {
		livingTimer = 0;
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
	}

	void Update(){
		livingTimer += Time.deltaTime;

		if (livingTimer >= lifeTime) {
			ObjectPool.Instance.ReleaseGameObject (gameObject);
		}
	}
	
}
