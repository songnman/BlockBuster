using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BottomWall : MonoBehaviour
{
	// Start is called before the first frame update
	public bool isBallStickBottom = true;
	public BlockManager blockManagerSc;

	void Start()
	{
		
	}

	private void OnCollisionEnter2D(Collision2D other) 
	{
		if(other.gameObject.CompareTag("Ball"))
		{
			Debug.Log("Found ball");
			isBallStickBottom = true;
			blockManagerSc.CreateBlockLineAndMove();
			other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
	private void OnCollisionExit2D(Collision2D other) 
	{
		// Debug.Log("Lost it");
		isBallStickBottom = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
