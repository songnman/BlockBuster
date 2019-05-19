using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public List<AudioSource> bgmSourceList;
	void Start()
	{
		// Instantiate(Resources.Load("Sounds/BGM/1") as AudioClip);
		bgmSourceList = new List<AudioSource>();
		int length = 7;
		for (int i = 0; i < length; i++)
		{
			bgmSourceList.Add(transform.GetChild(i).GetComponent<AudioSource>());
			bgmSourceList[i].clip = Resources.Load("Sounds/BGM/" + (i+1)) as AudioClip;
			bgmSourceList[i].loop = true;
			bgmSourceList[i].Play();
			if(i != 0)
				bgmSourceList[i].mute = true;
		}
	}
	public int hitBallCount	= 0;
	public void SoundQueueUp()
	{
		if(hitBallCount < 7)
			hitBallCount++;
		Debug.Log(hitBallCount);
		int length = hitBallCount;
		for (int i = 0; i < length; i++)
		{
			bgmSourceList[i].mute = false;
			
		}
	}	
	public void SoundReset()
	{
		hitBallCount = 0;
		int length = 7;
		for (int i = 0; i < length; i++)
		{
			if(i != 0)
				bgmSourceList[i].mute = true;
		}
	}
}
