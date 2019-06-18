using System.Collections;
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
		if (!isUsed1 && MenuManager.CountItem1 > 0)
			itemButtonList[0].interactable = true;
		if (!isUsed2 && MenuManager.CountItem2 > 0)
			itemButtonList[1].interactable = true;
		if (!isUsed3 && MenuManager.CountItem3 > 0)
			itemButtonList[2].interactable = true;
		if (!isUsed4 && MenuManager.CountItem4 > 0)
			itemButtonList[3].interactable = true;
		if (!isUsed5 && MenuManager.CountItem5 > 0)
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
		isUsed1 = true;
	}
	public void PierceFunc()
	{
		blockManagerSc.RigidBodySwitch(false);
		touchContolSc.isPierceActivate = true;
		itemButtonList[1].interactable = false;
		DeActivateItems();
		MenuManager.CountItem2--;
		MenuManager.SaveGame();
		isUsed2 = true;
	}	public void DoubleFunc()
	{
		touchContolSc.isDoubleActivate = true;
		itemButtonList[3].interactable = false;
		DeActivateItems();
		MenuManager.CountItem3--;
		MenuManager.SaveGame();
		isUsed3 = true;
	}
	public void BigBallFunc()
	{
		touchContolSc.isBigBallActivate = true;
		touchContolSc.firstBallObj.transform.localScale = new Vector3(0.6f,0.6f,1);
		itemButtonList[2].interactable = false;
		DeActivateItems();
		MenuManager.CountItem4--;
		MenuManager.SaveGame();
		isUsed4 = true;
	}
	public void ReAimFunc()
	{
		touchContolSc.isReaimActivate = true;
		itemButtonList[4].interactable = false;
		DeActivateItems();
		MenuManager.CountItem5--;
		MenuManager.SaveGame();
		isUsed5 = true;
	}
}
