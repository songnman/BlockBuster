using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Save
{
	public int CountItem1, CountItem2, CountItem3, CountItem4, CountItem5;
	public int skinNum;
	public int currency0, currency1;
	public int viewAdDay, viewAdMonth, viewAdCount, loginDay, loginMonth;
	public bool isGetDailyReward, isGetHottimeReward;
	public bool isSetAlarm, isAllowTerms;
	public List<bool> isHaveSkinList = new List<bool>();
}