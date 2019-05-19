using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBall : MonoBehaviour
{
	TouchContol touchContolSc;
	void Start() 
	{
		touchContolSc = GameObject.Find("TouchArea").GetComponent<TouchContol>();
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Bottom" || other.gameObject.tag == "Ball")
			Destroy(gameObject);
			touchContolSc.ballCount++;
	}
}
