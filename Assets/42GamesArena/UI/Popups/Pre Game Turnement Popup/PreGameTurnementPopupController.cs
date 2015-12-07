using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreGameTurnementPopupController : PopupBaseController {

	private System.Action _playButtonAction;

	[SerializeField]
	private GameObject _playButton;

	[SerializeField]
	private Text _timeLabel;

	private float _timeCount = 60;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		_timeCount -= Time.deltaTime;

		if (_timeCount < 0)
		{
			_timeCount = 60;
		}

		_timeLabel.text = "02:53:" + (int)_timeCount;
	}

	public void PlayButtonAction()
	{	
		_playButton.GetComponent<Button>().enabled = false;
		_playButton.GetComponent<Image>().color = Color.gray;
		if (_playButtonAction != null)
		{
			_playButtonAction();
		}
	}

	public void SetPlayButtonAction(System.Action playButtonAction)
	{
		_playButtonAction = playButtonAction;
	}


}
