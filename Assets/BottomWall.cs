using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BottomWall : MonoBehaviour
{
	// Start is called before the first frame update
	public bool isBallStickBottom = false;
	public TouchContol touchContolSc;
	void Start()
	{
		
	}
	private void OnCollisionEnter2D(Collision2D other) 
	{
		if(other.gameObject.CompareTag("Ball"))
		{
			if(other.gameObject.GetComponent<BallDefault>().isBallCollision)
			{
				touchContolSc.stickBallCount++;
				// Debug.Log(touchContolSc.stickBallCount);
				other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				if(!other.gameObject.GetComponent<BallDefault>().isCollisionBlock)
				{
					touchContolSc.collisionBlockFailCount++;
					// Debug.Log(touchContolSc.collisionBlockFailCount);
				}

				if(touchContolSc.firstBallObj != null)
				{
					touchContolSc.notFirstBallList.Add(other.gameObject);
				}
				else
				{
					touchContolSc.firstBallObj = other.gameObject;
					// Debug.Log("FirstBall Assigned");
				}
			}

		}
	}

	private static void NewMethod(Collision2D other)
	{
		Destroy(other.gameObject);
	}

	private void OnCollisionExit2D(Collision2D other) 
	{
		isBallStickBottom = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
