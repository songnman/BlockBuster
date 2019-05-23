﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public List<AudioSource> bgmSourceList;
	public List<AudioSource> fxSourceList;
	GameObject bgmPrefab;
	GameObject fxPrefab;
	void Start()
	{
		// Instantiate(Resources.Load("Sounds/BGM/1") as AudioClip);
		bgmSourceList = new List<AudioSource>();
		fxSourceList = new List<AudioSource>();
		bgmPrefab = gameObject.transform.GetChild(0).gameObject;
		fxPrefab = gameObject.transform.GetChild(1).gameObject;
		
		int bgmLength = bgmPrefab.transform.childCount;
		for (int i = 0; i < bgmLength; i++)
		{
			bgmSourceList.Add(bgmPrefab.transform.GetChild(i).GetComponent<AudioSource>());
			bgmSourceList[i].clip = Resources.Load("Sounds/BGM/" + (i+1)) as AudioClip;
			bgmSourceList[i].loop = true;
			bgmSourceList[i].Play();
			if(i != 0)
				bgmSourceList[i].volume = 0;
		}

		int fxLength = fxPrefab.transform.childCount;
		for (int i = 0; i < fxLength; i++)
		{
			fxSourceList.Add(fxPrefab.transform.GetChild(i).GetComponent<AudioSource>());
			// fxSourceList[i].clip = Resources.Load("Sounds/BGM/" + (i+1)) as AudioClip;
			// fxSourceList[i].loop = true;
			// fxSourceList[i].Play();
			// if(i != 0)
			// 	fxSourceList[i].volume = 0;
		}
	}
	public void PlayShoot()
	{
		fxSourceList[0].PlayOneShot(Resources.Load("Sounds/FX/Shoot") as AudioClip);
	}
	public void PlayHit()
	{
		// GameObject soundGo = new GameObject();
		// soundGo.AddComponent<AudioSource>().PlayOneShot(Resources.Load("Sounds/FX/Hit") as AudioClip);
		// yield return new WaitForSeconds(1.0f);
		// Destroy(soundGo);
		fxSourceList[1].PlayOneShot(Resources.Load("Sounds/FX/Hit2") as AudioClip);
		// fxSourceList[1].Play();
		
	}

	public int hitBallCount	= 0;
	public void SoundQueueUp()
	{
		if(hitBallCount < 6)
			hitBallCount++;

		int i = hitBallCount;
		object[] parms = new object[3] { bgmSourceList[i], 1.0f, 0.1f };
		StartCoroutine("ChangeSoundVolUp", parms);
	}	
	public void SoundReset()
	{
		hitBallCount = 0;
		int length = 7;
		for (int i = 0; i < length; i++)
		{
			if(i != 0)
			{
				object[] parms = new object[3] { bgmSourceList[i], 0.0f, 0.3f };
				StartCoroutine("ChangeSoundVolDown", parms);
			}

		}
	}
	IEnumerator ChangeSoundVolDown(object[] parms)
	{
		StopCoroutine("ChangeSoundVolUp");
		AudioSource audio = (AudioSource)parms[0];
		float audioVol = (float)parms[1];
		float changeInterval = (float)parms[2];
		for (int j = 0; j < 10; j++)
		{
			audio.volume = Mathf.Lerp ( audio.volume, audioVol, j * 0.1f );
			yield return new WaitForSeconds(changeInterval);
		}
	}	
	IEnumerator ChangeSoundVolUp(object[] parms)
	{
		StopCoroutine("ChangeSoundVolDown");
		AudioSource audio = (AudioSource)parms[0];
		float audioVol = (float)parms[1];
		float changeInterval = (float)parms[2];
		for (int j = 0; j < 10; j++)
		{
			audio.volume = Mathf.Lerp ( audio.volume, audioVol, j * 0.1f );
			yield return new WaitForSeconds(changeInterval);
		}
	}

}