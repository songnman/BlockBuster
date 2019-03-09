using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockDefault : MonoBehaviour
{
	public int leftCount = 3;
	public Text leftCountText;
	void Start()
	{
		leftCountText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
		leftCountText.text = leftCount.ToString("N0");
	}
	void Update()
	{
		
	}
	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Ball")
		{
			leftCount--;
			leftCountText.text = leftCount.ToString("N0");
			if(leftCount < 1)
			{
				if(gameObject.transform.parent.childCount < 2)
					Debug.Log("front");

				Destroy(gameObject);
			}
				
		}
	}
}
