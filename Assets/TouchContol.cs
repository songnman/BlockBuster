using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine;
using Spine.Unity;

public class TouchContol : MonoBehaviour
{
	[HideInInspector] public GameObject firstBallObj;
	[HideInInspector] public int ballCount;
	[HideInInspector]public Vector3 shootPos;
	public GameObject ballPrefab;
	public GameObject fakeBallPrefab;
	public GameObject increaseBallPrefab;
	public GameObject ballGroup;
	public BottomWall bottomWallSc;
	public GameObject dotLineObj;
	public BlockManager blockManagerSc;
	private LineRenderer ballLine;
	private LineRenderer touchLine;
	public bool isButtonDown = false;
	public int collisionBlockFailCount = 0;
	
	void Start()
	{
		Application.targetFrameRate = 60;
		ballLine = transform.GetChild(0).GetComponent<LineRenderer>();
		touchLine = transform.GetChild(1).GetComponent<LineRenderer>();
		// ballLine.material.mainTextureScale.Normalize();
		fakeBallPrefab.SetActive(false);
		skipButton.interactable = false;


		
		// firstBallObj = ballGroup.transform.GetChild(0).gameObject;
		
		firstBallObj = Instantiate(ballPrefab);
		firstBallObj.transform.SetParent(ballGroup.transform);
		firstBallObj.transform.position = new Vector2 (0, -3.4f);
		firstBallObj.transform.GetChild(0).GetComponent<SkeletonAnimation>().state.SetAnimation( 0, "Ball_off" , false);
		firstBallObj.transform.GetChild(0).GetComponent<SkeletonAnimation>().state.AddAnimation( 0, "Idle" , true, 0);

		ballCount = 1;
		bottomWallSc.isBallStickBottom = true;
	}	
	Vector3 oriTouchPos;
	Vector2 direction;
	Vector2 shootDirection;
	float BallSpeedFactor
	{
		get
		{
			if		(ballCount > 100)
				return 1.5f;
			else if	(ballCount > 200)
				return 2.0f;
			else
				return 1.0f;
		}
	}
	bool isSwipeEnable = false;
	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
	{
		if((Input.touchCount > 0 || touchFingerId > 0) && bottomWallSc.isBallStickBottom )
		{
			touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			touchPosition.z = 0f;

			switch (touchPhase)
			{
				case TouchPhase.Began:
					// Debug.Log("Began");
					if(touchPosition.y > 3.5f)
					{
						isSwipeEnable = false;
						break;
					}
					fakeBallPrefab.SetActive(true);
					shootPos = firstBallObj.transform.position;
					oriTouchPos = touchPosition;
					ballLine.positionCount = 2;
					touchLine.positionCount = 2;
					isSwipeEnable = true;
				break;

				case TouchPhase.Moved:
					if(!isSwipeEnable || touchPosition.y > 3.5f)
					{
						isSwipeEnable = false;
						fakeBallPrefab.SetActive(false);
						ballLine.positionCount = 0;
						touchLine.positionCount = 0;
						break;
					}
					// Debug.Log(touchPosition.y);
					touchLine.SetPosition(0, oriTouchPos);
					touchLine.SetPosition(1, touchPosition);
					ballLine.material.mainTextureOffset += Vector2.left * 0.1f;
					direction = touchPosition - oriTouchPos;
					direction = direction.normalized;
					float directionYLimit = 0.25f;
					if(direction.y > directionYLimit)
					{
						RaycastHit2D hits = Physics2D.CircleCast(shootPos, 0.21f, direction,100,LayerMask.GetMask("Block", "Block02", "Wall"));
						fakeBallPrefab.transform.position = hits.point;
						
						ballLine.SetPosition(0, shootPos);
						ballLine.SetPosition(1, hits.point);//(Vector2)shootPos.position + (direction * 20) );
						shootDirection = direction;
					}
					else
					{
						RaycastHit2D hits = Physics2D.CircleCast(shootPos, 0.21f, new Vector2(Mathf.Sign(direction.x), directionYLimit),100,LayerMask.GetMask("Block", "Block02", "Wall"));
						fakeBallPrefab.transform.position = hits.point;

						ballLine.SetPosition(0, shootPos);
						// ballLine.SetPosition(1, (Vector2)shootPos.position + new Vector2(Mathf.Sign(direction.x), directionYLimit) * 20 );
						ballLine.SetPosition(1, hits.point);
						shootDirection = (new Vector2(Mathf.Sign(direction.x), directionYLimit)).normalized;
					}
				break;

				case TouchPhase.Ended:

					// Debug.Log("End");
					fakeBallPrefab.SetActive(false);
					ballLine.positionCount = 0;
					touchLine.positionCount = 0;
					if(!isSwipeEnable || touchPosition.y > 3.5f)
						break;
					isSwipeEnable = false;
					//  ballCount = 10;
					StartCoroutine("ShootBall");
				break;
			}
		}
	}
	[HideInInspector]public int stickBallCount = 0;
	[HideInInspector]public int shootBallCount = 0;

	// [HideInInspector] public List<GameObject> ballList;
	[HideInInspector] public List<GameObject> notFirstBallList;
	public Text shootBallRemainText;
	public Text remainBallText;
	public Button skipButton;
	public void SkipShootBall()
	{
		stickBallCount = shootBallCount + 100;

		Debug.Log("shootBallCount " + shootBallCount);
		Debug.Log("stickBallCount " + stickBallCount);

	}
	public IEnumerator ShootBall()
	{
		bottomWallSc.isBallStickBottom = false;
		firstBallObj.transform.GetChild(0).GetComponent<SkeletonAnimation>().state.SetAnimation( 0, "Ball_on" , false);
		firstBallObj.transform.GetChild(0).GetComponent<SkeletonAnimation>().state.AddAnimation( 0, "loop" , false, 0);
		yield return new WaitForSeconds(0.4f);
		shootBallCount = ballCount;
		int shootBallRemain = ballCount;
		notFirstBallList = new List<GameObject>();
		List<GameObject> ballList = new List<GameObject>();
		
		float shootInterval = 0.03f;
		int airBallCount = ballGroup.transform.childCount - 1;

		Instantiate(Resources.Load("Particles/Ef_ball") as GameObject, firstBallObj.transform.position + new Vector3(0,-0.2f), Quaternion.identity);
		Destroy(firstBallObj);
		firstBallObj = null;
		
		for (int i = 0; i < shootBallCount; i++)
		{
			ballList.Add(Instantiate(ballPrefab, shootPos, Quaternion.identity));
			ballList[i].transform.SetParent(ballGroup.transform);

			if (ballCount > 100 && firstBallObj != null && stickBallCount > 10 && collisionBlockFailCount > 10)
				skipButton.interactable = true;

			if (stickBallCount >= shootBallCount)
				break;

			if (notFirstBallList.Count > 25)		//[2019-04-07 18:43:03] 일정량 이상의 볼이 돌아오면 저절로 회수. ( 최적화를 위해서 필요함)
			{
				GameObject targetBall = notFirstBallList[notFirstBallList.Count - 25];
				if (targetBall != null && targetBall.GetComponent<BallDefault>().isBallCollision)
				{
					// Debug.Log(targetBall);
					StartCoroutine("DestroyBalls", targetBall);
				}
			}

			ballList[i].GetComponent<Rigidbody2D>().AddForce(shootDirection * 700  * BallSpeedFactor );

			yield return new WaitForSeconds(shootInterval);

			shootBallRemain--;
			shootBallRemainText.text = "x" + shootBallRemain.ToString("N0");
			if (shootBallRemain < 1)
			{
				shootBallRemainText.gameObject.SetActive(false);
			}

			RemainBallCount();
		}
		// yield return new WaitUntil(()=> shootBallRemain < 1);
		// Debug.Log("shootBallCount " + shootBallCount);
		yield return new WaitUntil(()=> firstBallObj != null && (stickBallCount >= shootBallCount || blockManagerSc.gameObject.transform.childCount < 1)); // [2019-03-09 17:05:43] 마지막 공이 부착됐을 때.
		// Debug.Log("stickBallCount " + stickBallCount);

		skipButton.interactable = false;

		// Debug.Log("All Balls are Stuck");
		
		firstBallObj.transform.GetChild(0).GetComponent<SkeletonAnimation>().state.SetAnimation( 0, "Ball_off" , false);
		firstBallObj.transform.GetChild(0).GetComponent<SkeletonAnimation>().state.AddAnimation( 0, "Idle" , true, 0);

		foreach (GameObject item in ballList) //[2019-03-09 17:21:41] 차례대로 삭제되는 표현을 위해서 여지를 남김.
		{
			if(item != firstBallObj && item != null)
			{
				StartCoroutine("DestroyBalls" , item);
			}
		}
		Instantiate(Resources.Load("Particles/Ef_ball") as GameObject, firstBallObj.transform.position + new Vector3(0,-0.2f), Quaternion.identity);
		yield return new WaitForSeconds(1.167f);

		shootBallRemain = ballCount;
		shootBallRemainText.text = "x" + shootBallRemain.ToString("N0");
		shootBallRemainText.gameObject.SetActive(true);
		shootBallRemainText.transform.position = (Vector2)firstBallObj.transform.position + new Vector2(0, -0.5f);

		collisionBlockFailCount = 0;
		stickBallCount = 0;
		shootBallCount = 0;
		// Debug.Log(stickBallCount);
		bottomWallSc.isBallStickBottom = true;
		blockManagerSc.CreateBlockLineAndMove();
	}

	public void RemainBallCount()
	{
		remainBallText.text = "Remain Ball " + ballGroup.transform.childCount.ToString("N0");
	}

	IEnumerator DestroyBalls(GameObject item)
	{
		Vector3 oriPos = item.transform.position;
		for (int i = 0; i < 10; i++)
		{
			if(item != null)
				item.transform.position = Vector3.Lerp(oriPos, firstBallObj.transform.position, i * 0.1f);
			yield return new WaitForFixedUpdate();
		}
		Destroy(item);
		yield return null;
		RemainBallCount();
	}

	private void OnCollisionEnter2D(Collision2D other) {
		isButtonDown = true;
	}
	private void OnCollisionExit2D(Collision2D other) {
		isButtonDown = false;
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
	public void ShootCountInc()
	{
		ballCount += 10;
		blockManagerSc.shootCount += 10;
		blockManagerSc.shootCountText.text = blockManagerSc.shootCount.ToString("N0");
	}
}
