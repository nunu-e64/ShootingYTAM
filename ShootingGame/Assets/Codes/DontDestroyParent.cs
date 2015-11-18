using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngineExtra
{
	public class DontDestroyParent : MonoBehaviour
	{

#region UNITY_EVENT
		void Awake ()
		{
			if (Instance == this) {
				DontDestroyOnLoad (gameObject);
			} else {
				Destroy (gameObject);
			}
		}

		void OnLevelWasLoaded (int level)
		{
			transform.DetachChildren ();
			Destroy (gameObject);
			instance = null;
		}
#endregion

		public static void Register (GameObject obj)
		{
			obj.transform.parent = Instance.transform;
		}

		public static void Register (MonoBehaviour component)
		{
			Register (component.gameObject);
		}

		private static DontDestroyParent instance = null;

		public static DontDestroyParent Instance {
			get {
				if (instance == null) {
					instance = FindObjectOfType<DontDestroyParent> ();
					if (instance == null) {
						GameObject obj = new GameObject ("DontDestroyParent");
						instance = obj.AddComponent<DontDestroyParent> ();
					}
					instance.gameObject.hideFlags = HideFlags.NotEditable;
				}
				return instance;
			}
		}
	}
}

static class DontDestroyParentEX
{
	
	public static void DontDestroyOnNextLoad (this GameObject self, GameObject target)
	{
		UnityEngineExtra.DontDestroyParent.Register (target);
	}

	public static void DontDestroyOnNextLoad (this GameObject self, MonoBehaviour target)
	{
		UnityEngineExtra.DontDestroyParent.Register (target);
	}
	
	public static void DontDestroyOnNextLoad (this MonoBehaviour self, GameObject target)
	{
		UnityEngineExtra.DontDestroyParent.Register (target);
	}

	public static void DontDestroyOnNextLoad (this MonoBehaviour self, MonoBehaviour target)
	{
		UnityEngineExtra.DontDestroyParent.Register (target);
	}
}