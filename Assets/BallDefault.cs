using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDefault : MonoBehaviour
{
	public bool isBallCollision;
	private void OnCollisionEnter2D(Collision2D other) 
	{
		if(other.gameObject.tag != "Bottom")
		 isBallCollision = true;
	}
}
