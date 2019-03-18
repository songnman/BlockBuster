using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onButton : MonoBehaviour
{
	private void OnMouseEnter() {
		gameObject.transform.parent.GetComponent<TouchContol>().isButtonDown = true;
		Debug.Log("Enter Mouse");
	}
	private void OnMouseExit() {
		gameObject.transform.parent.GetComponent<TouchContol>().isButtonDown = false;
		Debug.Log("Exit Mouse");
	}

}
