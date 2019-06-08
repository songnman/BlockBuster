using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceDetect : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.GetComponent<BallDefault>().touchControlSc.isPierceActivate)
		{
			// Debug.Log("is on?");
			gameObject.transform.parent.GetComponent<BlockDefault>().DestroyBlock01();
		}
	}
}
