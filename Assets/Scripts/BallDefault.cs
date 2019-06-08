using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class BallDefault : MonoBehaviour
{
	public bool isBallCollision = false;
	public bool isCollisionBlock = false;
	[HideInInspector]public TouchContol touchControlSc;
	SoundManager soundManagerSc;

	void Start() 
	{
		GetComponent<Rigidbody2D>().velocity += GetComponent<Rigidbody2D>().velocity * touchControlSc.BallSpeedFactor * 0.01f;
	}
	private void OnCollisionEnter2D(Collision2D other) 
	{
		if(other.gameObject.tag != "Bottom")
		{
			isBallCollision = true;
		}

		if(other.gameObject.tag == "Block")
		{
			touchControlSc.hitBallCount++;
			Instantiate(Resources.Load("Particles/Ember") as GameObject, other.contacts[0].point, Quaternion.identity).transform.SetParent(touchControlSc.particlePrefab.transform);
			
			isCollisionBlock = true;
		}
	}
}
