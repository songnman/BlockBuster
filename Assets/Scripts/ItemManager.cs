﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
	public BlockManager blockManagerSc;
	public TouchContol touchContolSc;
	List<Button> itemButtonList = new List<Button>();
	bool isUsed1, isUsed2, isUsed3, isUsed4, isUsed5;
	void Start()
	{
		int length = gameObject.transform.childCount;
		for (int i = 0; i < length; i++)
		{
			itemButtonList.Add(gameObject.transform.GetChild(i).GetComponent<Button>());
			itemButtonList[i].interactable = false;
		}
		CheckItemMethod();
	}

	public void CheckItemMethod()
	{
		if (MenuManager.CountItem1 > 0 && !isUsed1)
			itemButtonList[0].interactable = true;
		if (MenuManager.CountItem2 > 0 && !isUsed2)
			itemButtonList[1].interactable = true;
		if (MenuManager.CountItem3 > 0 && !isUsed3)
			itemButtonList[2].interactable = true;
		if (MenuManager.CountItem4 > 0 && !isUsed4)
			itemButtonList[3].interactable = true;
		if (MenuManager.CountItem5 > 0 && !isUsed5)
			itemButtonList[4].interactable = true;
	}
	public void DeActivateItems()
	{
		int length = itemButtonList.Count;
		for (int i = 0; i < length; i++)
		{
			itemButtonList[i].interactable = false;
		}
	}
	public void MoveUpFunc()
	{
		int childCount = blockManagerSc.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			blockManagerSc.StartCoroutine("MoveUpBlock", i);
		}
		DeActivateItems();
		MenuManager.CountItem1--;
		MenuManager.SaveGame();
	}
	public void PierceFunc()
	{
		blockManagerSc.RigidBodySwitch(false);
		touchContolSc.isPierceActivate = true;
		itemButtonList[1].interactable = false;
		DeActivateItems();
		MenuManager.CountItem2--;
		MenuManager.SaveGame();
	}
	public void BigBallFunc()
	{
		touchContolSc.isBigBallActivate = true;
		touchContolSc.firstBallObj.transform.localScale = new Vector3(0.6f,0.6f,1);
		itemButtonList[2].interactable = false;
		DeActivateItems();
		MenuManager.CountItem3--;
		MenuManager.SaveGame();
	}
	public void DoubleFunc()
	{
		touchContolSc.isDoubleActivate = true;
		itemButtonList[3].interactable = false;
		DeActivateItems();
		MenuManager.CountItem4--;
		MenuManager.SaveGame();
	}
	public void ReAimFunc()
	{
		touchContolSc.isReaimActivate = true;
		itemButtonList[4].interactable = false;
		DeActivateItems();
		MenuManager.CountItem5--;
		MenuManager.SaveGame();
	}
}
