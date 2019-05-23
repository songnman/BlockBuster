using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBall : MonoBehaviour
{
	TouchContol touchContolSc;
	SoundManager soundManagerSc;
	void Start() 
	{
		touchContolSc = GameObject.Find("TouchArea").GetComponent<TouchContol>();
		soundManagerSc = GameObject.Find("Main").GetComponent<SoundManager>();
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Bottom" || other.gameObject.tag == "Ball")
			Destroy(gameObject);
		touchContolSc.ballCount++;
		soundManagerSc.PlayShoot();
		Instantiate(Resources.Load("Particles/Ef_ball") as GameObject, other.transform.position + new Vector3(0,-0.2f), Quaternion.identity);
	}
}
