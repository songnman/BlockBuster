using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BlockManager : MonoBehaviour
{
	float randomNumFloat = 0.3f;
	float RandomNumFloat
	{
		get
		{
			if		(shootCount + 1 < 50)
				return 0.3f;
			else if (shootCount + 1 < 100)
				return 0.4f;
			else if (shootCount + 1 < 150)
				return 0.5f;
			else if (shootCount + 1 < 200)
				return 0.6f;
			else if (shootCount + 1 < 250)
				return 0.7f;
			else if (shootCount + 1 < 300)
				return 0.8f;
			else
				return 0.9f;
		}
	}
	bool RandomNum
	{
		get
		{
			if(Random.Range(0,1f) < RandomNumFloat)
				return true;
			else
				return false;
		}
	}
	public GameObject blockPrefab;
	public GameObject block02Prefab;
	public GameObject bonusBallPrefab;
	public Text shootCountText;
	[HideInInspector]public int shootCount = 0;
	int maxBlockCount = 2;
	int MaxBlockCount
	{
		get
		{
			if		(shootCount + 2 > 10)
				return 3;
			else if (shootCount + 2 > 100)
				return 4;
			else if (shootCount + 2 > 200)
				return 5;
			else
				return 2;
		}
	}
	int blockHP = 1;
	int BlockHP
	{
		get
		{
			return shootCount + 1;
		}
	}
	public void CreateBlock02()
	{
		GameObject blockObj = Instantiate(block02Prefab, new Vector3(0, 0), Quaternion.Euler(0,0,0) );
		BlockDefault blockDefaultSc = blockObj.GetComponent<BlockDefault>();
		blockObj.transform.SetParent(gameObject.transform);
		
		blockDefaultSc.leftCount = (BlockHP - 1) * 2;
		blockDefaultSc.leftCountText = blockObj.transform.GetChild(0).GetChild(0).GetComponent<Text>();
		blockDefaultSc.leftCountText.text = blockDefaultSc.leftCount.ToString("N0");

		// int childCount = gameObject.transform.childCount;
		// for (int i = 0; i < childCount; i++)
		// {
		// 	gameObject.transform.GetChild(i).transform.position += new Vector3(0, -0.75f);
		// }

	}
	void Start()
	{
		CreateBlockLineAndMove();
	}
	public void CreateBlockLineAndMove()
	{
		bool isBonusBallExist = false;
		int blockCount = 0;
		List<int> blockLine = new List<int>();

		while(blockCount < 1 || !isBonusBallExist) //[2019-03-09 17:05:30] 블럭이 최소 1개 이상일때까지 계속.
		{
			for (int i = 0; i < 7; i++)
			{
				if (RandomNum && blockCount < MaxBlockCount && !blockLine.Contains(i))
				{
					GameObject blockObj = Instantiate(blockPrefab, new Vector3(i - 3, 3.0f), Quaternion.identity);
					BlockDefault blockDefaultSc = blockObj.GetComponent<BlockDefault>();
					blockObj.transform.SetParent(gameObject.transform);
	
					blockDefaultSc.leftCount = BlockHP;
					blockDefaultSc.leftCountText = blockObj.transform.GetChild(0).GetChild(0).GetComponent<Text>();
					blockDefaultSc.leftCountText.text = blockDefaultSc.leftCount.ToString("N0");
					blockLine.Add(i);
					blockCount++;
				}
				else if(RandomNum && !isBonusBallExist && !blockLine.Contains(i))
				{
					isBonusBallExist = true;
					GameObject bonusBallObj = Instantiate(bonusBallPrefab, new Vector3(i - 3, 3.0f), Quaternion.identity);
					// BonusBall bonusBallSc = bonusBallObj.GetComponent<BonusBall>();
					bonusBallObj.transform.SetParent(gameObject.transform);
					blockLine.Add(i);
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
	private void Update() {
		if(Input.GetKeyDown(KeyCode.F1))
			CreateBlockLineAndMove();
	}
}
