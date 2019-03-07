using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchContol : MonoBehaviour
{
	public GameObject ballObj;
	public BottomWall bottomWallSc;
	private LineRenderer ballLine;
	private LineRenderer touchLine;
	void Start()
	{
		ballLine = GetComponent<LineRenderer>();
		touchLine = ballObj.transform.GetChild(0).GetComponent<LineRenderer>();
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
					isSwipeEnable = true;
					oriTouchPos = touchPosition;
					ballLine.positionCount = 2;
					touchLine.positionCount = 2;
				break;

				case TouchPhase.Moved:
					if(!isSwipeEnable)
						break;
					touchLine.SetPosition(0, oriTouchPos);
					touchLine.SetPosition(1, touchPosition);
					direction = touchPosition - oriTouchPos;
					direction = direction.normalized;
					if(direction.y > 0.3)
					{
						ballLine.SetPosition(0, ballObj.transform.position);
						ballLine.SetPosition(1, (Vector2)ballObj.transform.position + (direction * 20) );
						shootDirection = direction;
					}
					else
					{
						ballLine.SetPosition(0, ballObj.transform.position);
						ballLine.SetPosition(1, (Vector2)ballObj.transform.position + new Vector2(Mathf.Sign(direction.x), 0.2f) * 20 );
						shootDirection = (new Vector2(Mathf.Sign(direction.x), 0.2f)).normalized;
					}
				break;

				case TouchPhase.Ended:
					if(!isSwipeEnable)
						break;
					bottomWallSc.isBallStickBottom = false;
					isSwipeEnable = false;
					ballLine.positionCount = 0;
					touchLine.positionCount = 0;
					ballObj.GetComponent<Rigidbody2D>().AddForce( shootDirection * 700 );
				break;
			}
		}
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
