using UnityEngine;
using System.Collections;

/// <summary>
/// 弾単体クラス
/// ・まっすぐ飛んでいく
/// </summary>
public class Bullet : MonoBehaviour {
	public float speed = 10;
	public float lifeTime = 5;

	[System.NonSerialized]
	public int[] bulletPower;
	public int power;
	
	private float livingTimer;

	void OnEnable () {

		bulletPower = new int[Mathf.Max (transform.childCount, 1)];
		
		for (int i = 0; i < bulletPower.Length; i++) {
			bulletPower[i] = power;			
		}

		livingTimer = 0;
		GetComponent<Rigidbody2D> ().velocity = transform.up.normalized * speed;
	}

	void Update(){
		livingTimer += Time.deltaTime;

		if (livingTimer >= lifeTime) {
			ObjectPool.Instance.ReleaseGameObject (gameObject);
		}
	}

	void SetSpeedRate () {

	}
	
}
