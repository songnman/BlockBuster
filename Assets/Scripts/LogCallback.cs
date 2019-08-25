using UnityEngine;
using UnityEngine.UI;

public class LogCallback : MonoBehaviour {
	public Text output;
	public Text stack;
	void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}
	void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}
	void HandleLog(string logString, string stackTrace, LogType type)
	{
		output.text = logString;
		stack.text = stackTrace;
	}
}
