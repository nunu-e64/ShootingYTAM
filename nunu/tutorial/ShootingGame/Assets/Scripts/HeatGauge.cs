using UnityEngine;
using System.Collections;

public class HeatGauge : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		//sr.bounds.size.x -= 0.5f;
		float width = sr.bounds.size.x;

		Debug.Log(width);

	}
}
