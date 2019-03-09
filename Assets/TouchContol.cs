using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchContol : MonoBehaviour
{
	[HideInInspector] public GameObject firstBallObj;
	[HideInInspector] public int ballCount;
	[HideInInspector]public Transform shootPos;
	public GameObject ballPrefab;
	public GameObject fakeBallPrefab;
	public GameObject increaseBallPrefab;
	public GameObject ballGroup;
	public BottomWall bottomWallSc;
	public GameObject dotLineObj;
	public BlockManager blockManagerSc;
	private LineRenderer ballLine;
	private LineRenderer touchLine;
	
	void Start()
	{
		ballLine = transform.GetChild(0).GetComponent<LineRenderer>();
		touchLine = transform.GetChild(1).GetComponent<LineRenderer>();
		
		// ballLine.material.mainTextureScale.Normalize();
		fakeBallPrefab.SetActive(false);

		
		// firstBallObj = ballGroup.transform.GetChild(0).gameObject;
		
		firstBallObj = Instantiate(ballPrefab);
		firstBallObj.transform.SetParent(ballGroup.transform);
		firstBallObj.transform.position = new Vector2 (0, -3.2f);
		ballCount = 1;
	}	
	Vector3 oriTouchPos;
	Vector2 direction;
	Vector2 shootDirection;
	bool isSwipeEnable = false;
	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
	{
		if((Input.touchCount > 0 || touchFingerId > 0) && bottomWallSc.isBallStickBottom)
		{
			touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			touchPosition.z = 0f;

			switch (touchPhase)
			{
				case TouchPhase.Began:
					// Debug.Log("Began");
					fakeBallPrefab.SetActive(true);
					shootPos = firstBallObj.transform;
					oriTouchPos = touchPosition;
					ballLine.positionCount = 2;
					touchLine.positionCount = 2;
					isSwipeEnable = true;
				break;

				case TouchPhase.Moved:
					if(!isSwipeEnable)
						break;
					touchLine.SetPosition(0, oriTouchPos);
					touchLine.SetPosition(1, touchPosition);
					ballLine.material.mainTextureOffset += Vector2.left * 0.1f;
					direction = touchPosition - oriTouchPos;
					direction = direction.normalized;
					if(direction.y > 0.2f)
					{
						RaycastHit2D hits = Physics2D.CircleCast(shootPos.position, 0.18f, direction,100,LayerMask.GetMask("Block"));
						fakeBallPrefab.transform.position = hits.point;
						
						ballLine.SetPosition(0, shootPos.position);
						ballLine.SetPosition(1, hits.point);//(Vector2)shootPos.position + (direction * 20) );
						shootDirection = direction;
					}
					else
					{
						RaycastHit2D hits = Physics2D.CircleCast(shootPos.position, 0.18f, new Vector2(Mathf.Sign(direction.x), 0.2f),100,LayerMask.GetMask("Block"));
						fakeBallPrefab.transform.position = hits.point;

						ballLine.SetPosition(0, shootPos.position);
						ballLine.SetPosition(1, (Vector2)shootPos.position + new Vector2(Mathf.Sign(direction.x), 0.2f) * 20 );
						shootDirection = (new Vector2(Mathf.Sign(direction.x), 0.2f)).normalized;
					}
				break;

				case TouchPhase.Ended:
					if(!isSwipeEnable)
						break;
					// Debug.Log("End");
					fakeBallPrefab.SetActive(false);
					isSwipeEnable = false;
					ballLine.positionCount = 0;
					touchLine.positionCount = 0;
					 ballCount = 10;
					StartCoroutine("ShootBall");
				break;
			}
		}
	}
	[HideInInspector]public int stickBallCount = 0;
	[HideInInspector] public List<GameObject> ballList;
	[HideInInspector] public List<GameObject> notFirstBallList;
	public Text shootBallRemainText;
	public IEnumerator ShootBall()
	{
		int shootBallCount = ballCount;
		int shootBallRemain = ballCount;
		notFirstBallList = new List<GameObject>();
		Destroy(firstBallObj);
		firstBallObj = null;
		bottomWallSc.isBallStickBottom = false;
		float shootInterval = 0.05f;
		ballList = new List<GameObject>();
		// ballList.Clear();
		for (int i = 0; i < shootBallCount; i++)
		{
			ballList.Add(Instantiate(ballPrefab));
			ballList[i].transform.SetParent(ballGroup.transform);
			ballList[i].transform.position = shootPos.position;
		}
		for (int i = 0; i < ballList.Count; i++)
		{
			ballList[i].GetComponent<Rigidbody2D>().AddForce( shootDirection * 700 );
			yield return new WaitForSeconds(shootInterval);
			shootBallRemain--;
			shootBallRemainText.text = "x" + shootBallRemain.ToString("N0");
			if(shootBallRemain < 1)
			{
				shootBallRemainText.gameObject.SetActive(false);
			}
		}
		
		yield return new WaitUntil(()=> firstBallObj != null && (stickBallCount == shootBallCount || blockManagerSc.gameObject.transform.childCount < 1)); // [2019-03-09 17:05:43] 마지막 공이 부착됐을 때.
		
		Debug.Log("All Balls are Stuck");
		foreach (GameObject item in ballList) //[2019-03-09 17:21:41] 차례대로 삭제되는 표현을 위해서 여지를 남김.
		{
			if(item != firstBallObj)
				Destroy(item);
			yield return new WaitForFixedUpdate();
		}
		shootBallRemain = ballCount;
		shootBallRemainText.text = "x" + shootBallRemain.ToString("N0");
		shootBallRemainText.gameObject.SetActive(true);
		shootBallRemainText.transform.position = (Vector2)firstBallObj.transform.position + new Vector2(0, -1);


		stickBallCount = 0;
		// Debug.Log(stickBallCount);
		bottomWallSc.isBallStickBottom = true;
		blockManagerSc.CreateBlockLineAndMove();
	}
	private void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			HandleTouch(touch.fingerId, touch.position, touch.phase);
		}
		// Simulate touch events from mouse events
		if ( Input.touchCount == 0 )
		{
			if (Input.GetMouseButtonDown(0))
			{
				HandleTouch(10, Input.mousePosition, TouchPhase.Began);
			}
			else if (Input.GetMouseButton(0))
			{
				HandleTouch(10, Input.mousePosition, TouchPhase.Moved);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				HandleTouch(10, Input.mousePosition, TouchPhase.Ended);
			}
		}
	}
}
