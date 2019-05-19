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
	// float randomRatio = 0.3f;
	bool RandomNum02
	{
		get
		{
			float randomRatio = 0;

			if(shootCount + 1 < 50)
				randomRatio = 0;
			else if (shootCount + 1 < 100)
				randomRatio = 0.30f;
			else if (shootCount + 1 < 150)
				randomRatio = 0.35f;
			else if (shootCount + 1 < 200)
				randomRatio = 0.40f;
			else if (shootCount + 1 < 250)
				randomRatio = 0.45f;
			else if (shootCount + 1 < 300)
				randomRatio = 0.50f;
			else if (shootCount + 1 < 350)
				randomRatio = 0.55f;
			else if (shootCount + 1 < 400)
				randomRatio = 0.60f;
			else if (shootCount + 1 < 450)
				randomRatio = 0.65f;
			else if (shootCount + 1 < 500)
				randomRatio = 0.70f;
			else if (shootCount + 1 < 550)
				randomRatio = 0.75f;
			else
				randomRatio = 0.80f;

			Debug.Log(randomRatio);

			if(Random.Range(0,1f) < randomRatio)
				return true;
			else
				return false;
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
		GameObject blockObj = Instantiate(block02Prefab, new Vector3(0, 0), Quaternion.Euler(0,0,45) );
		BlockDefault blockDefaultSc = blockObj.GetComponent<BlockDefault>();
		blockObj.transform.SetParent(gameObject.transform);
		
		object[] parms = new object[3]{ blockObj, 1.0f, 1.0f};
		StartCoroutine("IncBlockScale", parms);

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
			for (int i = 0; i < 6; i++)
			{
				if (RandomNum && blockCount < MaxBlockCount && !blockLine.Contains(i))
				{
					GameObject blockObj = Instantiate(blockPrefab, new Vector3(i - 2.5f, 3.75f), Quaternion.identity);
					BlockDefault blockDefaultSc = blockObj.GetComponent<BlockDefault>();
					blockObj.transform.SetParent(gameObject.transform);

					object[] parms = new object[3]{ blockObj,1.0f , 0.75f};
					StartCoroutine("IncBlockScale", parms);

					blockDefaultSc.leftCount = BlockHP;
					blockDefaultSc.leftCountText = blockObj.transform.GetChild(0).GetChild(0).GetComponent<Text>();
					blockDefaultSc.leftCountText.text = blockDefaultSc.leftCount.ToString("N0");
					blockLine.Add(i);
					blockCount++;
				}
				else if(RandomNum && !isBonusBallExist && !blockLine.Contains(i))
				{
					isBonusBallExist = true;
					GameObject bonusBallObj = Instantiate(bonusBallPrefab, new Vector3(i - 2.5f, 3.75f), Quaternion.identity);
					bonusBallObj.transform.SetParent(gameObject.transform);
					
					object[] parms = new object[3]{ bonusBallObj, 1.0f, 1.0f};
					StartCoroutine("IncBlockScale", parms);

					blockLine.Add(i);
				}
			}
		}
		blockCount = 0;

		int childCount = gameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			StartCoroutine( "MoveBlock", i );
		}

		if(shootCount > 50 && gameObject.transform.Find("Block02(Clone)") == null && RandomNum02)
			CreateBlock02();

		shootCount++;
		shootCountText.text = shootCount.ToString("N0");
	}

	IEnumerator MoveBlock(int i)
	{
		Vector3 oriPos = gameObject.transform.GetChild(i).transform.position;
		for (int j = 0; j < 10 + 1; j++)
		{
			gameObject.transform.GetChild(i).transform.position = Vector3.Lerp(oriPos, oriPos + new Vector3(0, -0.75f), j * 0.1f);
			yield return new WaitForFixedUpdate();
		}
		if (gameObject.transform.GetChild(i).transform.position.y < -2.5f)
			ResetGame();
	}

	public void ResetGame()
	{
		SceneManager.LoadScene("MainScene");
	}

	IEnumerator IncBlockScale(object[] parms)
	{
		
		GameObject go = (GameObject)parms[0];
		float posX = (float)parms[1];
		float posY = (float)parms[2];

		for (int j = 0; j < 10 + 1; j++)
		{
			go.transform.localScale = Vector2.Lerp(Vector3.zero, new Vector2(posX, posY), j * 0.1f);
			yield return new WaitForFixedUpdate();
		}
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.F1))
			CreateBlockLineAndMove();
	}
}
