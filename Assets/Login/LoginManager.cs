using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LoginManager : MonoBehaviour {

	[SerializeField] private Text txtLog;

	private void Awake()
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
			//로그인 성공
			Debug.Log(Social.localUser.userName);
			txtLog.text += "id : " + Social.localUser.id + "\n";
		}
		else
			Social.localUser.Authenticate((bool success) =>
			{
				if (success)
				{
					//로그인 성공
					Debug.Log(Social.localUser.userName);
					txtLog.text += "id : " + Social.localUser.id + "\n";
				}
				else
				{
					Debug.Log("로그인 실패");
					txtLog.text += "로그인 실패\n";
				}
			});
	}

	public void OnBtnQuitClicked()
	{
		Application.Quit();
	}
}
