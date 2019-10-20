using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class Test : MonoBehaviour
{
	RewardBasedVideoAd ad;
	string appId, unitId;
	public MenuManager sMenuManager;
	
	void Start ()
	{
		appId = "ca-app-pub-5838042073158481~1651838302";
		MobileAds.Initialize(appId);
		ad = RewardBasedVideoAd.Instance;
		
		//광고 요청이 성공적으로 로드되면 호출됩니다.
		ad.OnAdLoaded += OnAdLoaded;
		//광고 요청을 로드하지 못했을 때 호출됩니다.
		ad.OnAdFailedToLoad += OnAdFailedToLoad;
		//광고가 표시될 때 호출됩니다.
		ad.OnAdOpening += OnAdOpening;
		//광고가 재생되기 시작하면 호출됩니다.
		ad.OnAdStarted += OnAdStarted;
		//사용자가 비디오 시청을 통해 보상을 받을 때 호출됩니다.
		ad.OnAdRewarded += OnAdRewarded;
		//광고가 닫힐 때 호출됩니다.
		ad.OnAdClosed += OnAdClosed;
		//광고 클릭으로 인해 사용자가 애플리케이션을 종료한 경우 호출됩니다.
		ad.OnAdLeavingApplication += OnAdLeavingApplication;
		LoadAd();
	}
	
	void LoadAd()
	{
		AdRequest request = new AdRequest.Builder().Build();
		// unitId = "ca-app-pub-3940256099942544/5224354917"; //테스트 유닛 ID
		unitId = "ca-app-pub-5838042073158481/7502930788"; //가용 유닛ID
		ad.LoadAd(request, unitId);
    }

	public void OnBtnViewAdClicked()
	{
		if (DateTime.UtcNow.Day != MenuManager.viewAdDay || DateTime.UtcNow.Month != MenuManager.viewAdMonth)
			MenuManager.viewAdCount = 0;
		
		if (ad.IsLoaded())
		{
			if(MenuManager.viewAdCount < 10)
				ad.Show();
			else if(DateTime.UtcNow.Day != MenuManager.viewAdDay || DateTime.UtcNow.Month != MenuManager.viewAdMonth)
				MenuManager.viewAdCount = 0;
			else
				Debug.Log("Reward Limited");
		}
		else
		{
			LoadAd();
		}
	}
	static bool callbackCalled = false;
 
	//?[2019-08-28 01:30:20] 리워드 영상 시청 시 종료되는 현상을 콜백 함수로 해결 https://forum.unity.com/threads/crash-on-android-dismissing-rewarded-video-monetization-3-0-plugin.576064
	static void Callback()
	{
		callbackCalled = true;
	}
	void Update()
	{
		if( callbackCalled )
		{
			sMenuManager.AdViewRewardMethod();
			callbackCalled = false;
		}
	}
	void OnAdLoaded(object sender, EventArgs args) { Debug.Log("OnAdLoaded"); }
	void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e) { Debug.Log("OnAdFailedToLoad"); }
	void OnAdOpening(object sender, EventArgs e) { Debug.Log("OnAdOpening"); }
	void OnAdStarted(object sender, EventArgs e) { Debug.Log("OnAdStarted"); }
	static void OnAdRewarded(object sender, Reward e) 
	{
		callbackCalled = true;
		Debug.Log("OnAdRewarded");
	}
	void OnAdClosed(object sender, EventArgs e)
	{
		Debug.Log("OnAdClosed");
		LoadAd();
	}
	void OnAdLeavingApplication(object sender, EventArgs e) 
	{ 
		Debug.Log("OnAdLeavingApplication");
	}
}