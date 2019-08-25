using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LoginManager : MonoBehaviour {

	[SerializeField] private Text txtLog;
	// [SerializeField] private InputField inputScore;
	[SerializeField] private int inputScore;

	// private void Awake()
	// {
	// 	PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
	// 	PlayGamesPlatform.DebugLogEnabled = true;
	// 	PlayGamesPlatform.Activate();
	// }
	public void Start()
	{
		PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
	}

	public void OnBtnLoginClicked()
	{
		//이미 인증된 사용자는 바로 로그인 성공된다.
		if (Social.localUser.authenticated)
		{
			Debug.Log(Social.localUser.userName);
			// txtLog.text += "name : " + Social.localUser.userName + "\n";
		}
		else
			Social.localUser.Authenticate((bool success) =>
			{
				if (success)
				{
					Debug.Log(Social.localUser.userName);
					// txtLog.text += "name : " + Social.localUser.userName + "\n";
				}
				else
				{
					// txtLog.transform.parent.gameObject.SetActive(true);
					Debug.Log("Login Fail");
					// txtLog.text += "Login Fail\n";
				}
			});
	}

	public void OnBtnLogoutClicked()
	{
		((PlayGamesPlatform)Social.Active).SignOut();
		//PlayGamesPlatform.Instance.SignOut();
	}

	public void OnBtnQuitClicked()
	{
		Application.Quit();
	}

	public void OnBtnShowLeaderboardClicked()
	{
		//Social.ShowLeaderboardUI();
        //PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_1, LeaderboardTimeSpan.AllTime, (UIStatus status) =>
		((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard, LeaderboardTimeSpan.AllTime, (UIStatus status) =>
		{
			Debug.Log("status = " + status);
			// txtLog.text += "status = " + status;
		});

	}
	public void OnBtnReportScoreClicked(int scoreInt)
	{
		inputScore = scoreInt;
		//PlayGamesPlatform.Instance.ReportScore(int.Parse(txtScore.text), GPGSIds.leaderboard_1, (bool success) =>)
		//Social.ReportScore(int.Parse(inputScore.text), GPGSIds.leaderboard_1, (bool success) =>
		((PlayGamesPlatform)Social.Active).ReportScore(inputScore, GPGSIds.leaderboard, (bool success) =>
        {
			if (success)
			{
				Debug.Log("Report Success");
				// txtLog.text += "Report Success";
			}
			else
			{
				Debug.Log("Report Fail");
				// txtLog.text += "Report Fail";
			}
		});
	}
}
