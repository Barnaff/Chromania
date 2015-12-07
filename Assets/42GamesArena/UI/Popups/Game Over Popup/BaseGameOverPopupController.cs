using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseGameOverPopupController : PopupBaseController {
	
	[SerializeField]
	protected Text _scoreLabel;

	[SerializeField]
	protected Text _bestScoreLbel;

	[SerializeField]
	protected Text _topScoreLabel;

	private GameTileDataModel _game;

	private int _score;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region Public

	public void SetGameScore(GameTileDataModel game, int score)
	{
		_game = game;
		_score = score;
		RefreshGameOverPanel();

		Debug.Log("finished game: " + _game.GameDefenition.GameName);
	}

	#endregion


	#region Protected - User Interactions

	public virtual void HomeButtonAction()
	{
		ClosePopup(()=>
		           {
			Application.LoadLevel("Lobby Scene");
		});
	}

	public virtual void PlayAgainButtonAction()
	{
		ClosePopup(()=>
		           {
			GameLoaderUtil.ReplayCurrentGame();
		});
	}

	public virtual void ShareButtonAction()
	{

	}

	#endregion


	#region Protected

	protected virtual void RefreshGameOverPanel()
	{
		_scoreLabel.text = _score.ToString();


	}

	#endregion
}
