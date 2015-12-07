using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrozenGameplayManager : GameplayBase {

	#region Public Properties

	/// <summary>
	/// The ball prefab.
	/// </summary>
	public GameObject BallPrefab;

	/// <summary>
	/// The player controller.
	/// </summary>
	public GameObject PlayerController;

	/// <summary>
	/// The active balls.
	/// </summary>
	public List<FrozenBallController> ActiveBalls;

	/// <summary>
	/// The blocks drop down timer.
	/// </summary>
	public float BlocksDropDownTimer;

	#endregion


	#region Private Properties

	private bool _waitingToLaunchBall;

	private FrozenLayoutController _layoutController;
	
	private int _level = 0;

	#endregion


	#region GameplayBase Subclassing

	protected override void GameStarted (string GameVariables)
	{
		Debug.Log("game started");
		base.GameStarted (GameVariables);
		ActiveBalls = new List<FrozenBallController>();
		_layoutController = this.gameObject.GetComponent<FrozenLayoutController>() as FrozenLayoutController;
		_layoutController.OnAddScore += OnAddBlockScore;
		CreateBall();
		InvokeRepeating("DropBlocks", BlocksDropDownTimer, BlocksDropDownTimer);
	}

	protected override void GameOver ()
	{
		if (!_isGameOver)
		{
			foreach (FrozenBallController ballController in ActiveBalls)
			{
				DestoryBall(ballController);
			}
			ActiveBalls.Clear();
			base.GameOver ();
		}
	}

	protected override void KeepPlaying ()
	{
		base.KeepPlaying ();
		_layoutController.ResetBlocksContainer();
		CreateBall();
	}

	
	#endregion
	


	#region Update

	void FixedUpdate()
	{
		if (_waitingToLaunchBall && (Input.GetMouseButton(0) || Input.touchCount > 0))
		{
			LuanchBall();
		}
	}

	#endregion


	#region Public
	


	#endregion


	#region Private 

	private void DropBlocks()
	{
		if (!_isGameOver)
		{
			_layoutController.MoveDown();
			
			GameObject lowesBlock = _layoutController.GetLowesBlock();
			if (lowesBlock != null && lowesBlock.transform.position.y < PlayerController.transform.position.y)
			{
				GameOver();
			}
		}
	}

	private void CreateBall()
	{
		Debug.Log("create ball");
		Vector3 position = PlayerController.transform.position;
		position.y += 60.0f;
		GameObject newBall = Instantiate(BallPrefab, position, Quaternion.identity) as GameObject;
		newBall.name = "Ball";
		_waitingToLaunchBall = true;
		FrozenBallController ballController = newBall.GetComponent<FrozenBallController>() as FrozenBallController;
		ballController.OnHitTop += OnBallHitTop;
		ballController.OnHitBottom += OnBallHitBottom;
		ActiveBalls.Add(ballController);

		ballController.BallSpeed += _level * 42.0f;

	}

	private void LuanchBall()
	{
		if (ActiveBalls.Count == 0)
		{
			CreateBall();
		}

		FrozenBallController ball = ActiveBalls[0];
		ball.ShootBullet();
		_waitingToLaunchBall = false;
	}

	private void DestoryBall(FrozenBallController ballController)
	{
		if (ActiveBalls.Contains(ballController))
		{
			ActiveBalls.Remove(ballController);
		}
		Destroy(ballController.gameObject);
	}


	#endregion 


	#region Game Events

	private void OnBallHitTop(FrozenBallController ballController)
	{
		if (_layoutController != null)
		{
			_level++;
			DestoryBall(ballController);
			_layoutController.GetNewLayout(_level, ()=>
			{
				CreateBall();
			});

			//SoundUtil.PlaySound("New Level A", "New Level B");
		}
	}

	private void OnBallHitBottom(FrozenBallController ballController)
	{
		DestoryBall(ballController);
		GameOver();
	}

	private void OnAddBlockScore(int scoreToAdd)
	{
		AddScore(scoreToAdd);
	}

	#endregion
}
