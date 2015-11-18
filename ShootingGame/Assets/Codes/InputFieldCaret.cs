using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldCaret : MonoBehaviour, ISelectHandler {
	public void OnSelect (BaseEventData eventData) {
		InputField ipFld = GetComponent<InputField> ();
		if (ipFld != null) {
			RectTransform caretTransform = (RectTransform) transform.Find (gameObject.name + " Input Caret");
			caretTransform.anchoredPosition = new Vector2 (0, 20);  //調整
		}
	}
}
