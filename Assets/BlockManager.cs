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
		CreateBlockLine();
	}
	public void CreateBlockLine()
	{
		for (int i = -3; i < 4; i++)
		{
			if (RandomNum && blockCount < 6)
			{
				Instantiate(blockPrefab, new Vector3(i, 3.75f), Quaternion.identity).transform.SetParent(gameObject.transform);
				blockCount++;
			}
		}
		blockCount = 0;
		
		int childCount = gameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			gameObject.transform.GetChild(i).transform.position += new Vector3( 0 , -0.75f );
			if(gameObject.transform.GetChild(i).transform.position.y < -2.5f)
				SceneManager.LoadScene("MainScene");
		}
	}
	
}
