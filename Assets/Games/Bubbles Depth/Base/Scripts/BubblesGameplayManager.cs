using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class BubblesGameplayManager : GameplayBase {

	#region Public Properties

	public BubbleDepthCannonController Cannon;

	public string LevelDataFileName;

	public float GameTickTime;
	
	public GameObject BottomMarker;

	public GameObject BackgroundSprite;

	#endregion


	#region Private properties

	private BubbleDepthLayoutController _layoutController;

	private List<BubbleDepthLevelDefenition> _levels;

	private int _currentLevel;

	private float _gametimeTickCount;
	
	private float _gameTickMultiplier;

	#endregion


	#region GameplayBase Subclassing

	protected override void GameStarted (string GameVariables)
	{
		_layoutController = this.gameObject.GetComponent<BubbleDepthLayoutController>() as BubbleDepthLayoutController;
		_layoutController.OnLevelComplete += OnLevelComplete;
		_layoutController.OnShootFinished += OnShootFinished;
		_layoutController.OnAddScore += AddScore;
		loadLevels();
		_currentLevel = 1;
		Cannon.OnShootBall += OnShootBall;
		SpriteRenderer sr = BackgroundSprite.GetComponent<SpriteRenderer>() as SpriteRenderer;
		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;
		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		BackgroundSprite.transform.localScale = new Vector3( worldScreenWidth / width, worldScreenHeight / height, 1.0f);
		StartLevel(_currentLevel);
		base.GameStarted (GameVariables);
	}

	protected override void GameOver ()
	{
		Cannon.Paused = true;
		base.GameOver ();
	}

	protected override void KeepPlaying ()
	{
		_layoutController.MoveSinkingObjectUpForKeepPlaying();
		Cannon.Paused = false;
		base.KeepPlaying ();
	}
	
	#endregion
	



	#region Update

	void LateUpdate()
	{
		if (!_isGameOver)
		{
			_gametimeTickCount += Time.deltaTime;
			if (_gametimeTickCount > GameTickTime * _gameTickMultiplier)
			{
				_gametimeTickCount -= GameTickTime * _gameTickMultiplier;
				if (_layoutController != null)
				{
					_layoutController.DropContainer();
					GameObject lowestBall = _layoutController.GetLowestBall();
					if (lowestBall != null && lowestBall.transform.position.y < BottomMarker.transform.position.y)
					{
						GameOver();
					}
				}
			}
		}
	}

	#endregion


	#region private

	private void StartLevel(int levelId)
	{
		BubbleDepthLevelDefenition level = null;
		foreach (BubbleDepthLevelDefenition leveldefenition in _levels)
		{
			if (leveldefenition.LevelId == levelId)
			{
				level = leveldefenition;
				break;
			}
		}

		if (level != null)
		{
			Cannon.ClearCannon();
			_gameTickMultiplier = level.FallSpeed;
			_layoutController.StartLevel(level, ()=>
			{
				ArmCannon(true);
			});
		}
		else
		{
			Debug.LogError("Level: " + levelId + " Could not be found!");
		}

	
	}

	private void ArmCannon(bool firstShot = false)
	{
		GameObject ball = _layoutController.GetBallForCannon();
		if (firstShot)
		{
			GameObject secondBall = _layoutController.GetBallForCannon();
			Cannon.ArmCannon(ball, secondBall);
		}
		else
		{
			Cannon.ArmCannon(ball);
		}

	}

	private void loadLevels()
	{
		TextAsset textAsset = (TextAsset) Resources.Load(LevelDataFileName);  

		_levels = new List<BubbleDepthLevelDefenition>();
		
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml ( textAsset.text );
		XmlNode levels = xmldoc["Levels"];
		
		foreach (XmlNode levelnode in levels.ChildNodes)
		{
			BubbleDepthLevelDefenition level = new BubbleDepthLevelDefenition();

			level.LevelId = int.Parse(levelnode["LevelId"].InnerText);
			level.NumberOfColors = int.Parse(levelnode["NumberOfColors"].InnerText);
			level.NumberOfRows = int.Parse(levelnode["NumberOfRows"].InnerText);
			level.BoardWidth = int.Parse(levelnode["BoardWidth"].InnerText);
			level.InitialRows = int.Parse(levelnode["InitialRows"].InnerText);
			level.FallSpeed = float.Parse(levelnode["FallSpeed"].InnerText);
			_levels.Add(level);
		}
	}

	#endregion


	#region Events

	private void OnShootBall(GameObject ball)
	{
		if (_isGameOver)
		{
			_layoutController.ShootingBall(ball);
		}
	}

	private void OnLevelComplete()
	{
		_currentLevel++;
		_layoutController.ClearLevel(()=>
		{
			StartLevel(_currentLevel);
		});
	}

	private void OnShootFinished()
	{
		ArmCannon(false);
	}

	#endregion




}
