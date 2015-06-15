using UnityEngine;
using System.Collections;

public static class TimeManager {

	static bool isRunning= false;		//時間計測フラグ
	static int startTime = 0;

	public static void StartCount(){
		isRunning = true;
		startTime = System.Environment.TickCount;
	}

	public static int FinishCount () {
		if (isRunning) {
			return System.Environment.TickCount - startTime;
		} else{
			Debug.LogWarning ("TimerManager.FinishCount called but not started.");
			return 0;
		}
	}
}
