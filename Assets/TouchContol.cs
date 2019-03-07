﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchContol : MonoBehaviour, IPointerDownHandler
{
	public GameObject ballObj;
	public BottomWall bottomwallSc;
	private LineRenderer line;
	void Start()
	{
		line = GetComponent<LineRenderer>();
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		// Debug.Log("Touch!");
		// if(bottomwallSc.isBallStickBottom)
		// 	ballObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-500,500),Random.Range(500,800)) );
	}
	Vector3 oriTouchPos;
	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
	{
		if((Input.touchCount > 0 || touchFingerId > 0) && bottomwallSc.isBallStickBottom)
		{
			touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			touchPosition.z = 0f;

			switch (touchPhase)
			{
				case TouchPhase.Began:
					oriTouchPos = touchPosition;
					line.positionCount = 2;
				break;

				case TouchPhase.Moved:
					line.SetPosition(0, ballObj.transform.position);
					line.SetPosition(1, touchPosition);
					line.Simplify(0.02f);
				break;
				
				case TouchPhase.Ended:
					line.positionCount = 0;
					ballObj.GetComponent<Rigidbody2D>().AddForce(( touchPosition - ballObj.transform.position ) * 100 );
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
