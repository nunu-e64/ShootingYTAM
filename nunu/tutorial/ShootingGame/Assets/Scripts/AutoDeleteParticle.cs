using UnityEngine;
using System.Collections;

public class AutoDeleteParticle : MonoBehaviour {

	void Update () {
		if (!GetComponent<ParticleSystem> ().isPlaying) Destroy (gameObject);
	}
}
