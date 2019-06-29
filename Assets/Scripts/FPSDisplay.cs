using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class FPSDisplay : MonoBehaviour
{
    public int avgFrameRate;
    public Text display_Text;
	float current = 0;
 
    public void Update ()
    {
		if(current > .5f)
		{
			current = 0;
			avgFrameRate = (int)(1f / Time.unscaledDeltaTime);
			display_Text.text = avgFrameRate.ToString();
		}
		else
		{
			current += Time.deltaTime;
		}
    }
}