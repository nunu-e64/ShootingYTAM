using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Spaceship spaceship;

	// Use this for initialization
	IEnumerator Start () {  //Updateに書くと他の処理に影響を及ぼす恐れがあるためコルーチンを利用

        spaceship = GetComponent<Spaceship> ();

        while (true){
            spaceship.Shot(transform);
            yield return new WaitForSeconds(spaceship.shotDelay);
        }

	}
	
	// Update is called once per frame
	void Update () {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2 (x, y).normalized;
        spaceship.Move(direction);

	}
}
