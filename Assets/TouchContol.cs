using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchContol : MonoBehaviour, IPointerDownHandler
{
	public GameObject ballObj;
	public BottomWall bottomwallSc;
	void Start()
	{

	}
	public void OnPointerDown(PointerEventData eventData)
	{
		// Debug.Log("Touch!");
		if(bottomwallSc.isBallStickBottom)
			ballObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-500,500),Random.Range(500,800)) );
	}
	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) 
	{
		if((Input.touchCount > 0 || touchFingerId > 0))
		{
			switch (touchPhase)
			{
				case TouchPhase.Began:
				break;

				case TouchPhase.Moved:
				break;
				
				case TouchPhase.Ended:
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
