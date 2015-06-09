using UnityEngine;
using System.Collections;

/// <summary>
/// BGM管理クラス
/// ・シーン遷移時も破棄されない
/// ・元のシーンに戻った時に多重再生されないようにSingletonで実装
/// </summary>
public class Sound : MonoBehaviour {

	public bool dontDestroyEnabled = true;

	private static Sound instance = null;
	
	void Awake () {
		 if (instance != null && instance != this) {
			 Destroy(this.gameObject);
			 return;
		 } else {
			 instance = this;
		 }
		 DontDestroyOnLoad(this.gameObject);
	}

}
