// Sample written for for Spine 3.7
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this to the same GameObject as your SkeletonAnimation
public class SpineControl_Destroy : MonoBehaviour 
{
	void Start () 
	{
		StartCoroutine("DestroyAfterTime");
	}
	IEnumerator DestroyAfterTime()
	{
		yield return new WaitForSeconds(0.3f);
		Destroy(gameObject);
	}
}