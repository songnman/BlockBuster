using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomWall : MonoBehaviour
{
    // Start is called before the first frame update
	public bool isBallStickBottom = true;
    void Start()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D other) 
	{
		
		if(other.gameObject.CompareTag("Ball"))
		{
			Debug.Log("Found it");
			isBallStickBottom = true;
			other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}

	private void OnCollisionExit2D(Collision2D other) 
	{
		Debug.Log("Lost it");
		isBallStickBottom = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
