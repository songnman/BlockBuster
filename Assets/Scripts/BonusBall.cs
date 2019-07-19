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
		{
			
			touchContolSc.ballCount++;
			soundManagerSc.PlayShoot();
			if(gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
				gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

			Instantiate(Resources.Load("Particles/Ef_ball") as GameObject, other.transform.position + new Vector3(0,-0.2f), Quaternion.identity);
			Destroy(gameObject);
		}
		else if	(other.gameObject.tag == "Block" || other.gameObject.tag == "BonusBall")
		{
			gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		}

	}
}
