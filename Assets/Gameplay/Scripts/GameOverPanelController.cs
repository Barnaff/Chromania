using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOverPanelController : MonoBehaviour {

	#region Public Properties

	public Image GameOverTitle;

	#endregion


	#region Private

	private Action _gameOverAction;

	private Action _keepPlayingAction;

	#endregion


	#region Initialize

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
	}

	#endregion

	
	#region Public

	public void GameOver(Action gameOverAction, Action keepPlayingAction)
	{
		Debug.Log("game over!");
		_gameOverAction = gameOverAction;
		_keepPlayingAction = keepPlayingAction;

		this.gameObject.SetActive(true);
		GameOverTitle.gameObject.SetActive(true);

		iTween.PunchScale(GameOverTitle.gameObject, iTween.Hash("time", 1.0f, "x", 1.2f, "y", 1.2f, "oncompletetarget", this.gameObject, "oncomplete", "ShowGameOver"));
	}

	#endregion


	#region Private

	private void ShowGameOver()
	{
		iTween.ScaleTo(GameOverTitle.gameObject, iTween.Hash("time", 2.0f, "x", 1.5f, "y", 1.5f, "oncompletetarget", this.gameObject, "oncomplete", "FinishedGameOver"));
	}

	private void FinishedGameOver()
	{
		if (_gameOverAction != null)
		{
			_gameOverAction();
		}
	}

	#endregion
}
