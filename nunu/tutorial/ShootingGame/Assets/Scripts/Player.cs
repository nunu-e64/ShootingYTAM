using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 自機クラス
/// </summary>
public class Player : Spaceship {

	public ParticleSystem failShotEffect;	//チャージ不足の時のエフェクト

	[HeaderAttribute ("PlayerStatus")]
	public bool invincibleModeForTest = false;
	public bool canShot;
	public float shotNum = 1;			//砲台セット数(砲台1個＝弾2発(15/06/09現在))	//Animatorから変更するためにintではなくfloatでないといけない
	public float touchPosGapY = 1.0f;	//移動の際に指で機体が隠れないようにタップした位置からずらす値

	[System.Serializable]
	private class BulletData {
		public GameObject bullet;				//弾のプレハブ
		[TooltipAttribute ("ChargeBorders=0%~100%")]
		public int chargeBorder;				//弾が出るチャージ量
		public ParticleSystem chargeEffect;		//チャージ中のエフェクト
	}

	[SerializeField, HeaderAttribute ("BulletStatus")]
	private BulletData[] bullets;		//弾のプレハブ

	[System.NonSerialized]
	public bool IsAppearance;

	private AudioSource shotAudio;
	private Animator animator;
	private GaugeManager gaugeManager;
	private bool isCharging;
	private int currentChargeIndex = -1;

	private Vector2 oldTouchPosition;
	private Vector2 currentTouchPosition;
	private Vector2 oldPosition;

	void Start () {
		shotAudio = GetComponent<AudioSource> ();
		gaugeManager = FindObjectOfType<GaugeManager> ();
		gaugeManager.SetPlayer (this);
		animator = GetComponent<Animator>();

		foreach (BulletData bullet in bullets){
			bullet.chargeEffect.Stop ();
			//Clear
		}


		/*
		//弾発射ループ
        while (true){

			if (canShot && !IsAppearance) {

				//子要素を全て取得して弾を発射
				for (int i = 0; i < transform.childCount && i < (int)shotNum; i++) {
					Transform shotPosition = transform.GetChild(i);
					Shot(shotPosition);
				}

				//弾発射SE再生
				if (shotNum > 0) {
					if (shotAudio.isActiveAndEnabled) shotAudio.Play ();
					else Debug.LogWarning ("OK");
				}
			}
			yield return new WaitForSeconds(shotDelay);
        }
		*/
	}

	void Update () {

		//チャージ段階によってエフェクトを切り替える//////////////////////////////
		if (isCharging) {
			int count = gaugeManager.GetCount();
			int newIndex = -1;

			for (int i = 0; i < bullets.Length; i++) {
				if (count >= bullets[i].chargeBorder) {
					newIndex = i;
				}
			}

			if (currentChargeIndex != newIndex) {
				if (currentChargeIndex >= 0) bullets[currentChargeIndex].chargeEffect.Stop ();
				if (newIndex >= 0) bullets[newIndex].chargeEffect.Play ();
				currentChargeIndex  = newIndex;
			}
		}
		

		//////////////////////////////////////////////////////////////////////////

		//チャージ開始と解除//////////////////////////////////////////////////////
		if ((Input.GetMouseButton (0) || Input.GetKey (KeyCode.Z)) && !isCharging) {
			gaugeManager.BeginCharge ();
			isCharging = true;
			currentChargeIndex = -1;


		//} else if ((Input.GetMouseButton (0) || Input.GetKey (KeyCode.Z)) && isCharging) {
		//	int count  = gaugeManager.GetCount();
		//	if (count < 10) {
		//	} else if (count < 40) {
		//		chargeEffect.startColor
		//	} else if (count < 70) {
		//		Shot (bullets[1]);
		//	} else if (count < 100) {
		//		Shot (bullets[2]);
		//	} else if (count == 100) {
		//		Shot (bullets[3]);
		//	} else {
		
		}else if ((Input.GetMouseButtonUp (0) || Input.GetKeyUp (KeyCode.Z)) && isCharging) {
			gaugeManager.EndCharge ();
			isCharging = false;
			
			if (currentChargeIndex == -1) {
				GameObject go = (GameObject) Instantiate (failShotEffect.gameObject, transform.position, transform.rotation);
				go.transform.parent = transform;
			} else {
				bullets[currentChargeIndex].chargeEffect.Stop ();
				bullets[currentChargeIndex].chargeEffect.Clear ();
				Shot (bullets[currentChargeIndex].bullet);
			}

			currentChargeIndex = -1;
		}
		//////////////////////////////////////////////////////////////////////////


		if (!IsAppearance) {
	
			//DEBUG: 自殺/////////////////////////////////////////////////////////////
			if (Input.GetMouseButtonDown (2) || Input.GetKeyDown (KeyCode.Escape)) {
				OnDead ();
			}
			//////////////////////////////////////////////////////////////////////////

			//入力に基づいて目標座標を求め移動////////////////////////////////////////
			if (Input.GetMouseButtonDown (0)) {
				oldTouchPosition = Camera.main.ScreenToWorldPoint ((Vector2) Input.mousePosition);
				oldPosition = (Vector2) transform.position;
			}

			Vector2 targetWorldPosition;
			Vector2 direction = new Vector2();

			if (Input.GetMouseButton (0)) {	//マウス入力
				currentTouchPosition = (Vector2) Camera.main.ScreenToWorldPoint ((Vector2) Input.mousePosition);
				direction =  currentTouchPosition - oldTouchPosition;
				targetWorldPosition = (Vector2) oldPosition + direction;

			} else {	//キー入力
				targetWorldPosition = new Vector2 (transform.position.x, transform.position.y) + new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")) * speed * Time.deltaTime / 5;
			}

			Move (targetWorldPosition);
			//////////////////////////////////////////////////////////////////////////


		} else {
			transform.position += new Vector3 (0, 0.07f, 0);
		}
	}


	//移動＋移動制限
	void Move(Vector2 targetPos) {
		
		//画面左下と右上のワールド座標をカメラのビューポート（0~1）から変換して取得
		Vector2 min = (Vector2) Camera.main.ViewportToWorldPoint (new Vector2 (0, 0)) +new Vector2 (0.3f, 0.3f);
		Vector2 max = (Vector2) Camera.main.ViewportToWorldPoint (new Vector2 (1, 1)) + new Vector2 (-0.3f, -0.3f);
		
		Vector2 pos = transform.position;
		Vector2 header = (targetPos - (Vector2)transform.position);
		float moveDistance = speed * Time.deltaTime;


		//目標座標が近ければ目標座標に自機座標を一致させ, 目標座標が遠ければSpeed*Time.deltaTimeだけ移動
		if (header.sqrMagnitude < moveDistance * moveDistance) {
			pos = targetPos;
		} else {
			Vector2 direction = header.normalized;
			pos += direction * moveDistance;
		}

		Vector2 fixedPos = new Vector2(Mathf.Clamp(pos.x, min.x, max.x), Mathf.Clamp(pos.y, min.y, max.y));
		if (fixedPos.x != pos.x) {
			oldTouchPosition.x = currentTouchPosition.x;
			oldPosition.x = fixedPos.x;
		}
		if (fixedPos.y != pos.y) {
			oldTouchPosition.y = currentTouchPosition.y;
			oldPosition.y = fixedPos.y;
		}


		transform.position = fixedPos;
	}

	
	void Shot(GameObject bullet){
		//子要素を全て取得して弾を発射
		for (int i = 0; i < transform.childCount && i < (int)shotNum; i++) {
			Transform shotPosition = transform.GetChild(i);
			Instantiate (bullet, shotPosition.position, shotPosition.rotation);
		}

		//弾発射SE再生
		if (shotNum > 0) {
			if (shotAudio.isActiveAndEnabled) shotAudio.Play ();
			else Debug.LogWarning ("AudioErrorHasAvoided: OK");
		}
	}

	//自機登場演出終了処理
	public void FinishAppearance () {

		if (Input.GetMouseButton (0)) {	//マウス入力)
			gaugeManager.BeginCharge ();
			isCharging = true;
		}

		if (Input.GetMouseButton (0)) {	//マウス入力
			oldTouchPosition = Camera.main.ScreenToWorldPoint ((Vector2) Input.mousePosition);
			oldPosition = transform.position;
		}

	}

	//被弾判定
	void OnTriggerEnter2D(Collider2D c) {

		//レイヤーに応じて処理を分岐
		string layerName = LayerMask.LayerToName(c.gameObject.layer);

		if (layerName == "Bullet(Enemy)") {
			ObjectPool.Instance.ReleaseGameObject(c.gameObject);
		}

		if (layerName == "Bullet(Enemy)" || layerName == "Enemy") {
			if (!invincibleModeForTest) {
				OnDead ();
			} else {
				Explosion ();
			}
		}

	}

	//死亡処理		
	public void OnDead () {	
		Explosion ();
		Destroy (gameObject);

		FindObjectOfType<Manager> ().StartCoroutine(FindObjectOfType<Manager> ().GameOver ());
	}

	//特殊攻撃
	public void InvokeSpecialAttack(){
		animator.SetTrigger("Special");
	}

	//死亡演出
	public override void Explosion () {
		for (int i = 0; i < 6; i++) {
			GameObject go = Instantiate (explosion, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),transform.position.y + Random.Range(-0.52f, 0.48f)), transform.rotation) as GameObject;
			go.GetComponent<Explosion> ().SetTimer (i * 0.2f);
		}
	}
}
