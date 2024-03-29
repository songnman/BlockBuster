﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopWall : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other) 
	{
		if(other.gameObject.CompareTag("Ball"))
		{
			float randomRange = Random.Range(-5.0f,5.0f);
			other.gameObject.GetComponent<Rigidbody2D>().AddForce( Vector2.left * randomRange );

			if(other.gameObject.GetComponent<BallDefault>().touchControlSc.isPierceActivate == true)
				other.gameObject.GetComponent<BallDefault>().touchControlSc.PierceDeActivate();
		}
	}

}
