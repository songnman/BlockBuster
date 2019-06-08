using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	public BlockManager blockManagerSc;
	public TouchContol touchContolSc;

	public void MoveUpFunc()
	{		
		int childCount = blockManagerSc.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			blockManagerSc.StartCoroutine( "MoveUpBlock", i );
		}
	}
	public void PierceFunc()
	{
		blockManagerSc.RigidBodyOff();
		touchContolSc.isPierceActivate = true;
	}
}
