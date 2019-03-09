using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BlockManager : MonoBehaviour
{
	public GameObject blockPrefab;
	List<GameObject> blockList = new List<GameObject>();
	float randomNumFloat = 0.3f;

	bool RandomNum
	{
		get
		{
			if(Random.Range(0,1f) < randomNumFloat)
				return true;
			else
				return false;
		}
	}
	int blockCount = 0;
	void Start()
	{
		CreateBlockLineAndMove();
	}
	public void CreateBlockLineAndMove()
	{
		while(blockCount < 1)
		{
			for (int i = 0; i < 7; i++)
			{
				if (RandomNum)
				{
					Instantiate(blockPrefab, new Vector3(i - 3, 3.0f), Quaternion.identity).transform.SetParent(gameObject.transform);
					blockCount++;
				}
			}		
		}
		blockCount = 0;

		int childCount = gameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			gameObject.transform.GetChild(i).transform.position += new Vector3(0, -0.75f);
			if (gameObject.transform.GetChild(i).transform.position.y < -2.5f)
				SceneManager.LoadScene("MainScene");
		}
	}
}
