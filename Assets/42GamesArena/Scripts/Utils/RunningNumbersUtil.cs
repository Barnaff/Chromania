using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RunningNumbersUtil : MonoBehaviour {

	[SerializeField]
	private bool isRunning = false;

	public static void RunNumbers(Text textField, int toNumber, float duration, string format = "", System.Action completionAction = null)
	{
		RunningNumbersUtil runningNumbersUtil = null;
		if (textField.gameObject.GetComponent<RunningNumbersUtil>() != null)
		{
			runningNumbersUtil = textField.gameObject.GetComponent<RunningNumbersUtil>();
		}

		if (runningNumbersUtil == null)
		{
			runningNumbersUtil = textField.gameObject.AddComponent<RunningNumbersUtil>();
		}

		if (runningNumbersUtil != null)
		{
			runningNumbersUtil.RunNumbers(toNumber, duration, format, completionAction);
		}
	}

	public void RunNumbers(int toNumber, float duration, string format, System.Action completionAction)
	{
		StartCoroutine(RunNumbersCorutine(toNumber, duration, format, completionAction));
	}

	IEnumerator RunNumbersCorutine(int toNumber, float duration, string format, System.Action completionAction)
	{
		if (isRunning)
		{
			StopAllCoroutines();
			isRunning = false;
			RunNumbersCorutine(toNumber, duration, format, completionAction);
			yield break;
		}

		Text textField = this.GetComponent<Text>();
		int currentNumber = int.Parse(textField.text.Replace(",",""));
		isRunning = true;
		float timeCount = 0.0f;

		while (timeCount < duration)
		{
			timeCount += Time.deltaTime;
			float numbet = Mathf.Lerp(currentNumber, toNumber, timeCount);
			textField.text = Mathf.Floor(numbet).ToString(format);
			yield return null;
		}

		textField.text = toNumber.ToString(format);

		isRunning = false;


	}
}
