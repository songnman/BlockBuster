﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDefault : MonoBehaviour
{
	public bool isBallCollision = false;
	public bool isCollisionBlock = false;
	private void OnCollisionEnter2D(Collision2D other) 
	{
		
		if(other.gameObject.tag != "Bottom")
			isBallCollision = true;

		if(other.gameObject.tag == "Block")
		{
			
			Instantiate(Resources.Load("Particles/Ember"), other.contacts[0].point, Quaternion.identity);
			isCollisionBlock = true;
		}

	}
}
