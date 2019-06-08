using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaimControl : MonoBehaviour
{
	bool isSwipeEnable = false;
	public TouchContol touchContolSc;
	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
	{
		if((Input.touchCount > 0 || touchFingerId > 0) && touchContolSc.isReaimActivate)
		{
			touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			touchPosition.z = 0f;

			switch (touchPhase)
			{
				case TouchPhase.Began:
					if(touchPosition.y > 3.5f)
					{
						isSwipeEnable = false;
						break;
					}
					isSwipeEnable = true;
				break;

				case TouchPhase.Moved:
					if(!isSwipeEnable || touchPosition.y > 3.5f)
					{
						isSwipeEnable = false;
						break;
					}
					if(Mathf.Abs(touchPosition.x) > 2.6f)
					{
						if(touchPosition.x > 0)
							touchContolSc.firstBallObj.transform.position = new Vector3(2.6f,touchContolSc.firstBallObj.transform.position.y);
						else
							touchContolSc.firstBallObj.transform.position = new Vector3(-2.6f,touchContolSc.firstBallObj.transform.position.y);
					}
					else
					{
						touchContolSc.firstBallObj.transform.position = new Vector3(touchPosition.x,touchContolSc.firstBallObj.transform.position.y);
					}
						
					Debug.Log(touchPosition.x);
				break;

				case TouchPhase.Ended:
					if(!isSwipeEnable || touchPosition.y > 3.5f)
						break;
					isSwipeEnable = false;
					touchContolSc.isReaimActivate = false;
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
