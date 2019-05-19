using UnityEngine;
using Spine.Unity;
public class Spine_Control_Test : MonoBehaviour
{
	public SkeletonAnimation otherSkeletonData;
	[SpineAnimation(dataField: "otherSkeletonData")] 
	public string firstState;
	[SpineAnimation(dataField: "otherSkeletonData")] 
	public string[] stateArray;
	void Start()
	{
		SkeletonAnimation spineAnim = GetComponent<SkeletonAnimation>();
		spineAnim.state.SetAnimation( 0, firstState, false);
		
		int length = stateArray.Length;
		for (int i = 0; i < length; i++)
		{
			GetComponent<SkeletonAnimation>().state.AddAnimation( 0, stateArray[i] , false, 0);
		}
	}
}