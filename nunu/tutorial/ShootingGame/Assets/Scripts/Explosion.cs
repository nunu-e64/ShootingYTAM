using UnityEngine;using System.Collections;

/// <summary>
/// PlayerとEnemyの爆破演出
/// </summary>public class Explosion : MonoBehaviour {	void OnAnimationFinish() {		Destroy(gameObject);	}}