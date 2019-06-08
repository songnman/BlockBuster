using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBall : MonoBehaviour
{
	public TouchContol touchContolSc;
	public SoundManager soundManagerSc;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Ball")
			Destroy(gameObject);
		touchContolSc.ballCount++;
		soundManagerSc.PlayShoot();
		Instantiate(Resources.Load("Particles/Ef_ball") as GameObject, other.transform.position + new Vector3(0,-0.2f), Quaternion.identity);
	}
}
