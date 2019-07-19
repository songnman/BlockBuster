﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other) 
	{
		if(other.gameObject.CompareTag("Ball"))
		{
			other.gameObject.GetComponent<Rigidbody2D>().AddForce( Vector2.down * 5 );
			
			if(other.gameObject.GetComponent<BallDefault>().touchControlSc.isPierceActivate == true)
				other.gameObject.GetComponent<BallDefault>().touchControlSc.PierceDeActivate();
		}
	}
}