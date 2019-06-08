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
		if(GameObject.Find("TouchArea") != null)
		{
			touchControlSc = GameObject.Find("TouchArea").GetComponent<TouchContol>();
			GetComponent<Rigidbody2D>().velocity += GetComponent<Rigidbody2D>().velocity * touchControlSc.BallSpeedFactor * 0.01f;
		}

		// soundManagerSc = GameObject.Find("Main").GetComponent<SoundManager>();
	}
	private void OnCollisionEnter2D(Collision2D other) 
	{
		
		if(other.gameObject.tag != "Bottom")
		{
			isBallCollision = true;

			// Vector2 velo = GetComponent<Rigidbody2D>().velocity;
			// if( Mathf.Abs(velo.x) < 15 && Mathf.Abs(velo.y) < 15 )
			// 	GetComponent<Rigidbody2D>().velocity += GetComponent<Rigidbody2D>().velocity * touchControlSc.BallSpeedFactor * 0.01f;

			// Debug.Log(GetComponent<Rigidbody2D>().velocity);

		}
			

		if(other.gameObject.tag == "Block")
		{
			touchControlSc.hitBallCount++;
			Instantiate(Resources.Load("Particles/Ember") as GameObject, other.contacts[0].point, Quaternion.identity);
			isCollisionBlock = true;
			// if(touchControlSc.hitBallCount > 0 && touchControlSc.hitBallCount < 7)
			// 	soundManagerSc.SoundQueue(touchControlSc.hitBallCount);

		}

	}
}
