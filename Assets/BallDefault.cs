using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

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
			GameObject particle;
			particle = Instantiate(Resources.Load("Particles/Ef_ball") as GameObject, gameObject.transform.position, Quaternion.identity);

			SkeletonAnimation skeleton = particle.GetComponent<SkeletonAnimation>();
			isCollisionBlock = true;
		}

	}
}
