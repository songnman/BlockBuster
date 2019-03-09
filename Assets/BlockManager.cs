using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BlockManager : MonoBehaviour
{
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
	public GameObject blockPrefab;
	public GameObject bonusBallPrefab;
	public Text shootCountText;
	int blockCount = 0;
	int shootCount = -1;
	int maxBlockCount = 2;
	int blockHP = 1;
	int BlockHP
	{
		get
		{
			return shootCount + 2;
		}
	}
	void Start()
	{
		CreateBlockLineAndMove();
	}
	public void CreateBlockLineAndMove()
	{
		while(blockCount < 1) //[2019-03-09 17:05:30] 블럭이 최소 1개 이상일때까지 계속.
		{
			for (int i = 0; i < 7; i++)
			{
				if (RandomNum && blockCount < maxBlockCount)
				{
					GameObject blockObj = Instantiate(blockPrefab, new Vector3(i - 3, 3.0f), Quaternion.identity);
					BlockDefault blockDefaultSc = blockObj.GetComponent<BlockDefault>();
					blockObj.transform.SetParent(gameObject.transform);
					
					blockDefaultSc.leftCount = BlockHP;
					blockDefaultSc.leftCountText = blockObj.transform.GetChild(0).GetChild(0).GetComponent<Text>();
					blockDefaultSc.leftCountText.text = blockDefaultSc.leftCount.ToString("N0");
					
					blockCount++;
				}
				else if(RandomNum)
				{
					GameObject bonusBallObj = Instantiate(bonusBallPrefab, new Vector3(i - 3, 3.0f), Quaternion.identity);
					// BonusBall bonusBallSc = bonusBallObj.GetComponent<BonusBall>();
					bonusBallObj.transform.SetParent(gameObject.transform);
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
		shootCount++;
		shootCountText.text = shootCount.ToString("N0");
	}
}
