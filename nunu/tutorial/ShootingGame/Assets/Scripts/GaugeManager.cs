using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ゲージの状態によって自機が弾を打てるかを切り替えるための関数群
/// ・AnimationEventから呼び出す
/// </summary>
public class GaugeManager : MonoBehaviour {

	public GameObject EnergyGauge;

	[SerializeField]
	private Text gaugeText;

	public float count = 0.00f;		//ゲージのカウントダウン表示。Animationで値変化

	private Player player = null;
	private bool isCharging;

	private Animator animator;


	void Awake () {
		animator = GetComponent<Animator> ();
	}

	void Start () {	

		//Textの位置をゲージに合わせる
		gaugeText.transform.position = (Vector2) Camera.main.WorldToScreenPoint (transform.position);

		count = 0.0f;

	}


	void Update () {
		
		if (true) {
			string tmpText = count.ToString ("00.0") + "%";
			gaugeText.text = tmpText;
		}

	}

	void OnEnable () {
		gaugeText.gameObject.SetActive (true);
		isCharging = false;
		animator.ResetTrigger ("charge");
		animator.ResetTrigger ("release");
	}

	void OnDisable () {
		if (!gaugeText.IsDestroyed ()) {
			gaugeText.gameObject.SetActive (false);
			EnergyGauge.transform.position = new Vector3 (-0.5f, EnergyGauge.transform.position.y, EnergyGauge.transform.position.z);
			EnergyGauge.transform.localScale = new Vector3 (0.0f, EnergyGauge.transform.localScale.y, EnergyGauge.transform.localScale.z);
			count = 0.0f;
		}
	}

	public void SetPlayer(Player _player) {
		player = _player;
	}

	public void BeginCharge () {

		if (!isCharging) {
			isCharging = true;
			animator.SetTrigger ("charge");
			gaugeText.gameObject.SetActive (true);
		}

	}

	public int EndCharge () {

		if (isCharging) {
			isCharging = false;
			animator.SetTrigger ("release");
		}
		return (int) count;

	}

	public int GetCount () {
		return (int) count;
	}


}
