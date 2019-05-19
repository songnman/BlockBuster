﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockDefault : MonoBehaviour
{
	public int leftCount = 3;
	public Text leftCountText;
	public GameObject particle;
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
		if		(other.gameObject.tag == "Ball")
		{
			// if(gameObject.name != "Block02(Clone)")
			// 	Instantiate(Resources.Load("Particles/Ef_block"), gameObject.transform.position, Quaternion.identity);

			// Debug.Log("Ball Collision!");
			if(gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
				gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			leftCount--;
			leftCountText.text = leftCount.ToString("N0");
			if(leftCount < 1)
			{
				// if(gameObject.transform.parent.childCount < 2)
				// 	Debug.Log("front");
				if(gameObject.name != "Block02(Clone)")
				Instantiate(Resources.Load("Particles/Ef_block"), gameObject.transform.position, Quaternion.identity);
				Destroy(gameObject);
			}
		}
		else if	(other.gameObject.tag == "Block")
		{
			gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			// Debug.Log("Block Collision!");
		}
	}
}