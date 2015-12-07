using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BJPopupsController : MonoBehaviour {

	[SerializeField]
	private GameObject _overlayPanel;

	[SerializeField]
	private GameObject _youWinPanel;
	
	[SerializeField]
	private Text _youWinCoinsLabel;

	[SerializeField]
	private GameObject _gameplayMessage;



	public void Init()
	{
		_overlayPanel.SetActive(false);
		_youWinPanel.SetActive(false);
		if (_gameplayMessage != null)
		{
			_gameplayMessage.SetActive(false);
		}
	}


	public IEnumerator DisplayYouWin(int coinsAmount)
	{
		yield return StartCoroutine(DisplayWinCorutine(coinsAmount));
	}

	public IEnumerator DisplayGameplayMessage(string message)
	{
		yield return StartCoroutine(DisplayGameplayMessageCorutine(message));
	}


	private void DispalyOverlay()
	{
		_overlayPanel.SetActive(true);
		_overlayPanel.GetComponent<Image>().CrossFadeAlpha(0.5f, 0.5f, false);
	}

	IEnumerator HideOverlay()
	{
		_overlayPanel.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.5f, false);

		yield return new WaitForSeconds(0.5f);
		_overlayPanel.SetActive(false);
	}


	IEnumerator DisplayWinCorutine(int coinsAmount)
	{
		DispalyOverlay();

		_youWinPanel.SetActive(true);
		_youWinPanel.transform.localScale = new Vector3(1,1,1);
		iTween.PunchScale(_youWinPanel, iTween.Hash("time", 0.5f, "x", 1.2f, "y", 1.2f));
		_youWinCoinsLabel.text = coinsAmount.ToString();

		yield return new WaitForSeconds(1.3f);

		iTween.ScaleTo(_youWinPanel, iTween.Hash("time", 0.2f, "x", 0, "y", 0, "easetype", iTween.EaseType.easeInElastic));

		StartCoroutine(HideOverlay());

		yield return new WaitForSeconds(0.2f);
		_overlayPanel.SetActive(false);  

	}

	IEnumerator DisplayGameplayMessageCorutine(string message)
	{
		_gameplayMessage.SetActive(true);
		_gameplayMessage.GetComponent<Text>().text = message;
		DispalyOverlay();
		Vector3 position = _gameplayMessage.transform.position;

		iTween.MoveFrom(_gameplayMessage, iTween.Hash("time", 1.5f, "y", Screen.height, "easetype", iTween.EaseType.easeOutElastic, "islocal", true));

		yield return new WaitForSeconds(1.5f);

		iTween.MoveTo(_gameplayMessage, iTween.Hash("time", 0.5f, "y", - 300,  "islocal", true));
		StartCoroutine(HideOverlay());
		yield return new WaitForSeconds(0.5f);

		_gameplayMessage.transform.position = position;
		_gameplayMessage.SetActive(false);


	}
}
