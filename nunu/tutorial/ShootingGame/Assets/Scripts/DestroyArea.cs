using UnityEngine;
using System.Collections;

public class DestroyArea : MonoBehaviour {

	void OnTriggerExit2D(Collider2D c) {
		//Debug.Log(LayerMask.LayerToName(c.gameObject.layer));
		if (LayerMask.LayerToName(c.gameObject.layer) == "Player") Debug.LogFormat("pos: {0}, {1}", c.transform.position.x, c.transform.position.y);
		Destroy(c.gameObject);
	}

}
