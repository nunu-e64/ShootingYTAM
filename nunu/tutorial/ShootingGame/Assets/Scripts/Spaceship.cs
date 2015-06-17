using UnityEngine;
using System.Collections;

//TIPS: 必須コンポーネント（この記述がある限り該当コンポーネントを削除できない）
[RequireComponent(typeof(Rigidbody2D))]


/// <summary>
/// PlayerとEnemyが継承する機体汎用クラス	
/// ・移動と死亡時の爆発描画
/// ・移動速度など変数の管理
/// </summary>
public class Spaceship : MonoBehaviour {

	[HeaderAttribute ("ShipStatus")]
	public float speed;				//機体の移動速度[Unity/s]

	[HeaderAttribute ("OtherRef")]
	public GameObject explosion;
	
	public void Explosion() {
		Instantiate(explosion, transform.position, transform.rotation);
	}

}
