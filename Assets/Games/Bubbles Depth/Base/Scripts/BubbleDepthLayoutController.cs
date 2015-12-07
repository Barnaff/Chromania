using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void LevelCompleteDelegate();

public delegate void ShootFinishedDeleagte();

public delegate void AddScoreDelegate(int scoreToAdd);

public class BubbleDepthLayoutController : MonoBehaviour {

	#region Public Properties

	public int BoardWidth;

	public int BoardHeight;

	public GameObject[] BallsPrefabs;

	public int NumberOfColors;
	
	public GameObject BombPrefab;

	public LevelCompleteDelegate OnLevelComplete; 

	public ShootFinishedDeleagte OnShootFinished;

	public AddScoreDelegate OnAddScore;

	#endregion


	#region Private Properties

	private List<GameObject> _ballsPool = new List<GameObject>();

	private List<GameObject> _bombsPool = new List<GameObject>();

	private List<GameObject> _currentUsedBallsPrefabs;

	private Vector3 _leftTopCorner;

	private float _screenWidth;

	private GameObject _boardContainer;

	private List<BubbleDepthBallController> _allBalls = new List<BubbleDepthBallController>();

	private List<GameObject> _allBombs;

	private float _ballScale;

	private int _lowestRow = 0;

	private float _topMarginY;

	private bool _pauseDecending;
	
	#endregion


	#region Initialize
	// Use this for initialization
	void Awake () 
	{
		_leftTopCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f));
		_leftTopCorner.z = 0;
		_screenWidth = Mathf.Abs(_leftTopCorner.x) * 2.0f;
	}

	#endregion
	

	#region Public

	public void StartLevel(BubbleDepthLevelDefenition level, Action completionAction)
	{
		BoardWidth = level.BoardWidth;
		BoardHeight = level.NumberOfRows;
		NumberOfColors = level.NumberOfColors;

		SelectColors();

		LoadLayout();

		float initialSize = _ballScale * (BoardHeight - level.InitialRows);
		Vector3 boardPosition = _boardContainer.transform.position;

		boardPosition.y += initialSize;
		_boardContainer.transform.localPosition = boardPosition;



		iTween.MoveFrom(_boardContainer, iTween.Hash("time", 0.5f, "y", _boardContainer.transform.position.y + 6.0f));

		completionAction();

		_pauseDecending = false;
	}

	public GameObject GetBallForCannon()
	{
		GameObject ball = CreateBall(0,0);
		BubbleDepthBallController ballController = ball.GetComponent<BubbleDepthBallController>() as BubbleDepthBallController;
		if (ballController != null)
		{
			ballController.OnBallHit += OnBallHit;
		}
		return ball;
	}

	public void DropContainer()
	{
		if (!_pauseDecending)
		{
			Vector3 boardPosition = _boardContainer.transform.position;
			boardPosition.y -= 0.1f;
			_boardContainer.transform.localPosition = boardPosition;
		}
	}

	public void ShootingBall(GameObject ball)
	{
		ball.transform.SetParent(_boardContainer.transform);
	}

	public void ClearLevel(Action completionAction)
	{
		StartCoroutine(DropAllBalls(completionAction));
		_pauseDecending = true;
	}

	public GameObject GetLowestBall()
	{
		GameObject lowestBall = null;
		foreach (BubbleDepthBallController ball in _allBalls)
		{
			if (lowestBall == null)
			{
				lowestBall = ball.gameObject;
			}
			else
			{
				if (lowestBall.transform.position.y > ball.gameObject.transform.position.y)
				{
					lowestBall = ball.gameObject;
				}
			}
		}
		return lowestBall;
	}

	public void MoveSinkingObjectUpForKeepPlaying()
	{
		iTween.MoveBy(_boardContainer, iTween.Hash("time", 0.9f, "y", 5.0f));
		_pauseDecending = true;
	}

	#endregion


	#region Private

	IEnumerator DropAllBalls(Action completionAction)
	{
		_pauseDecending = true;
		List<BubbleDepthBallController> tempAllBalls = new List<BubbleDepthBallController>(_allBalls);
		foreach (BubbleDepthBallController ball in tempAllBalls)
		{
			_allBalls.Remove(ball);
			ball.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
			ball.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
			ball.gameObject.GetComponent<Collider2D>().isTrigger = true;
			ball.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-40,40), 60));
			if (OnAddScore != null)
			{
				OnAddScore(7);
			}
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(1.6f);
		 
		completionAction();
	}

	private void LoadLayout()
	{
		if (_boardContainer == null)
		{
			_boardContainer = new GameObject();
			_boardContainer.name = "Board Container";
		}
		_boardContainer.transform.position = _leftTopCorner;

		_topMarginY = _leftTopCorner.y;

		for (int y=0; y < BoardHeight; y++)
		{
			for (int x = 0; x< BoardWidth; x++)
			{
				GameObject ball = CreateBall(x,y);
				_allBalls.Add(ball.GetComponent<BubbleDepthBallController>() as BubbleDepthBallController);
			}
		}
		if (BombPrefab != null)
		{
			_allBombs = new List<GameObject>();
			for (int i=0; i < BoardWidth ; i++)
			{
				GameObject bomb = GetBomb();
					
				Vector3 position =  PositionForBall(new Vector2(i, -1));
				position.x -= _ballScale * 0.5f - (i * _ballScale * 0.5f / BoardWidth);
				position.y += _ballScale * 0.5f;

				bomb.transform.SetParent(_boardContainer.transform);
				bomb.transform.localPosition = position;
				bomb.transform.localScale = new Vector3(_ballScale, _ballScale, _ballScale);
				bomb.name = "Bomb";
				_allBombs.Add(bomb);
			}
		}


		//iTween.MoveFrom(_boardContainer, iTween.Hash("time", 0.5f, "y", _boardContainer.transform.position.y + 5 , "easeType", iTween.EaseType.easeOutElastic));
	}

	private GameObject CreateBall(int posX, int posY)
	{
		GameObject ball = GetBall();
		ball.transform.SetParent(_boardContainer.transform);

		if (posY < _lowestRow)
		{
			_lowestRow = posY;
		}

		ball.transform.localPosition = PositionForBall(new Vector2(posX, posY));

		BubbleDepthBallController ballController = ball.GetComponent<BubbleDepthBallController>() as BubbleDepthBallController;
		if (ballController != null)
		{
			ballController.PositionInBoard = new Vector2(posX, posY);
			ballController.IsShifted = (posY % 2 == 0);

		}
		return ball;
	}

	private void SelectColors()
	{
		_currentUsedBallsPrefabs = new List<GameObject>();

		List<GameObject> tempColors = new List<GameObject>(BallsPrefabs);

		do
		{
			GameObject randomColor = tempColors[UnityEngine.Random.Range(0, tempColors.Count)];
			_currentUsedBallsPrefabs.Add(randomColor);
			tempColors.Remove(randomColor);
		} while (_currentUsedBallsPrefabs.Count < NumberOfColors);

	}

	private GameObject GetBall(GameObject ballPrefab = null)
	{
		if (ballPrefab == null)
		{
			ballPrefab = _currentUsedBallsPrefabs[UnityEngine.Random.Range(0,_currentUsedBallsPrefabs.Count)];
		}

		foreach (GameObject ballInPool in _ballsPool)
		{
			if (ballInPool != null && ballInPool.name == ballPrefab.name && !ballInPool.activeInHierarchy)
			{
				ballInPool.SetActive(true);
				ballInPool.GetComponent<Collider2D>().isTrigger = false;
				ballInPool.GetComponent<Rigidbody2D>().isKinematic = true;
				ballInPool.GetComponent<Rigidbody2D>().gravityScale = 0;
				_ballsPool.Remove(ballInPool);	
				return ballInPool;
			}
		}

		GameObject ball = Instantiate(ballPrefab) as GameObject;
		float ballSize = _screenWidth / (BoardWidth + 0.5f);
		_ballScale = ballSize / ball.GetComponent<SpriteRenderer>().bounds.size.x;
		ball.name = ballPrefab.name;

		BubbleDepthBallController ballController = ball.GetComponent<BubbleDepthBallController>() as BubbleDepthBallController;
		if (ballController != null)
		{
			ballController.OnHitBottom += DestroyBallController;
			ballController.OnHitSinkingObject += OnBallHitSinkingObject;
			ballController.OnBallHitBomb += OnBallHitBomb;
			ballController.IsShifted = false;
			ballController.Checked = false;
		}

		ball.transform.localScale = new Vector3(_ballScale, _ballScale, _ballScale);
		ball.SetActive(true);
		return ball;
	}

	private GameObject GetBomb()
	{
		foreach (GameObject bomb in _bombsPool)
		{
			if (!bomb.activeInHierarchy)
			{
				bomb.SetActive(true);
				return bomb;
			}
		}

		GameObject newBomb = Instantiate(BombPrefab) as GameObject;
		newBomb.SetActive(true);
		_bombsPool.Add(newBomb);
		return newBomb;
	}

	private void DestroyBall(GameObject ball)
	{
		ball.SetActive(false);
		_ballsPool.Add(ball);
	}

	private void DestroyBallController(BubbleDepthBallController ballController)
	{
		if (_allBalls.Contains(ballController))
		{
			_allBalls.Remove(ballController);
		}
		DestroyBall(ballController.gameObject);
	}
	
	IEnumerator CalculateHit(BubbleDepthBallController ball)
	{
		_pauseDecending = true;
		List<BubbleDepthBallController> touchingBalls = GetTouchingSameColorBalls(ball);

		if (touchingBalls.Count > 2)
		{

			foreach (BubbleDepthBallController touchingBall in touchingBalls)
			{
				touchingBall.gameObject.GetComponent<Collider2D>().isTrigger = true;
				_allBalls.Remove(touchingBall);
			}

			List<BubbleDepthBallController> floatingBalls = GetFloatingBalls();

			foreach (BubbleDepthBallController floatingBall in floatingBalls)
			{
				floatingBall.gameObject.GetComponent<Collider2D>().isTrigger = true;
				_allBalls.Remove(floatingBall);

			}

			if (OnShootFinished != null)
			{
				OnShootFinished();
			}


			int ballCount = 0;
			foreach (BubbleDepthBallController touchingBall in touchingBalls)
			{
				touchingBall.GetComponent<BubbleDepthBallController>().KillBall();


				DestroyBall(touchingBall.gameObject);

				if (OnAddScore != null)
				{
					if (ballCount < 7)
					{
						ballCount++;
					}
					OnAddScore(ballCount);
				}

				yield return new WaitForEndOfFrame();
			}

			yield return new WaitForEndOfFrame();
			

			
			foreach (BubbleDepthBallController floatingBall in floatingBalls)
			{
				floatingBall.GetComponent<Rigidbody2D>().isKinematic = false;
				floatingBall.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
				floatingBall.GetComponent<Collider2D>().isTrigger = true;
				floatingBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-40,40), 60));
				if (OnAddScore != null)
				{
					OnAddScore(3);
				}
				yield return new WaitForEndOfFrame();
			}
			
			
			GameObject lowestBall = GetLowestBall();
			if (lowestBall.transform.position.y > _topMarginY)
			{
				float distance = lowestBall.transform.position.y - _leftTopCorner.y;
				iTween.MoveTo(_boardContainer, iTween.Hash("time", 0.5, "y", _boardContainer.transform.position.y - distance - 4.0f));
				yield return new WaitForSeconds(0.5f);
			}

		}
		else
		{
			if (OnShootFinished != null)
			{
				OnShootFinished();
			}
		}
		_pauseDecending = false;
	}

	private void PositionNewBall(BubbleDepthBallController ball, BubbleDepthBallController hit)
	{
		ball.transform.SetParent(_boardContainer.transform);
		Vector2 slotPosition =  GetClosestFreePosition(ball.gameObject.transform.localPosition, hit);
		ball.PositionInBoard = slotPosition;
		if (ball.PositionInBoard.y % 2 == 0)
		{
			ball.IsShifted = true;
		}
		else
		{
			ball.IsShifted = false;
		}
		Vector3 positionForBall = PositionForBall(slotPosition);
		ball.transform.localPosition = positionForBall;

		_allBalls.Add(ball);
	}

	private Vector2 GetClosestFreePosition(Vector3 ballPosition, BubbleDepthBallController ballHit)
	{
		Vector2[] sroundingSlots = new Vector2[6];
		if (ballHit.IsShifted)
		{
			sroundingSlots[0] = new Vector2(-1,-1);
			sroundingSlots[1] = new Vector2(0,-1);
			sroundingSlots[2] = new Vector2(-1,0);
			sroundingSlots[3] = new Vector2(1,0);
			sroundingSlots[4] = new Vector2(0,1);
			sroundingSlots[5] = new Vector2(-1,1);
		}
		else
		{
			sroundingSlots[0] = new Vector2(1,-1);
			sroundingSlots[1] = new Vector2(0,-1);
			sroundingSlots[2] = new Vector2(-1,0);
			sroundingSlots[3] = new Vector2(1,0);
			sroundingSlots[4] = new Vector2(0,1);
			sroundingSlots[5] = new Vector2(1,1);
		}

		List<Vector2> freeSlots = new List<Vector2>();
		foreach (Vector2 slot in sroundingSlots)
		{
			Vector2 slotPosition = slot + ballHit.PositionInBoard;
			if (slotPosition.x >= 0 && slotPosition.x < BoardWidth)
			{
				BubbleDepthBallController ballInSlot = GetBallForPosition(slotPosition);
				if (ballInSlot == null)
				{
					freeSlots.Add(slotPosition);
				}
			}
		}

		float minDistance = Mathf.Infinity;
		Vector2 minDistancePosition = Vector2.zero;

		foreach (Vector2 slot in freeSlots)
		{
			Vector3 realSlotPosition = PositionForBall(slot);
			float distance = Vector3.Distance(ballPosition, realSlotPosition);
			if (distance < minDistance)
			{
				minDistance = distance;
				minDistancePosition = slot;
			}
		}

		return minDistancePosition;
	}

	private BubbleDepthBallController GetBallForPosition(Vector2 positionInBoard)
	{
		foreach (BubbleDepthBallController ball in _allBalls)
		{
			if (ball.PositionInBoard == positionInBoard)
			{
				return ball;
			}
		}
		return null;
	}

	private Vector3 PositionForBall(Vector2 ballPosition)
	{
		Vector3 placePosition = new Vector3(ballPosition.x * _ballScale, -ballPosition.y * _ballScale - (_ballScale * 0.5f) ,0.0f);
		if (ballPosition.y % 2 == 0)
		{
			placePosition.x += 0.5f * _ballScale;
		}
		else
		{
			placePosition.x += _ballScale;
		}
		return placePosition;
	}

	private List<BubbleDepthBallController> GetTouchingSameColorBalls(BubbleDepthBallController ball)
	{
		foreach (BubbleDepthBallController b in _allBalls)
		{
			b.Checked = false;
		}

		List<BubbleDepthBallController> touchingBalls = new List<BubbleDepthBallController>();
		List<BubbleDepthBallController> ballsToCheck = new List<BubbleDepthBallController>();
		ballsToCheck.Add(ball);
		touchingBalls.Add(ball);
		do 
		{
			List<BubbleDepthBallController> sroundingBalls = GetSroundingBalls(ballsToCheck[0]);
			ballsToCheck.RemoveAt(0);
			foreach (BubbleDepthBallController sroundingBall in sroundingBalls)
			{
				if (sroundingBall != null && !sroundingBall.Checked)
				{
					if (sroundingBall.gameObject.name == ball.gameObject.name)
					{
						if (!touchingBalls.Contains(sroundingBall))
						{
							touchingBalls.Add(sroundingBall);
						}
						ballsToCheck.Add(sroundingBall);
					}
					sroundingBall.Checked = true;
				}
			}
		} while(ballsToCheck.Count > 0);

		return touchingBalls;
	}

	private List<BubbleDepthBallController> GetSroundingBalls(BubbleDepthBallController ball)
	{
		Vector2[] sroundingSlots = new Vector2[6];
		if (ball.IsShifted)
		{
			sroundingSlots[0] = new Vector2(-1,-1);
			sroundingSlots[1] = new Vector2(0,-1);
			sroundingSlots[2] = new Vector2(-1,0);
			sroundingSlots[3] = new Vector2(1,0);
			sroundingSlots[4] = new Vector2(0,1);
			sroundingSlots[5] = new Vector2(-1,1);
		}
		else
		{
			sroundingSlots[0] = new Vector2(1,-1);
			sroundingSlots[1] = new Vector2(0,-1);
			sroundingSlots[2] = new Vector2(-1,0);
			sroundingSlots[3] = new Vector2(1,0);
			sroundingSlots[4] = new Vector2(0,1);
			sroundingSlots[5] = new Vector2(1,1);
		}

		List<BubbleDepthBallController> stoundingBalls = new List<BubbleDepthBallController>();
		foreach (Vector2 slot in sroundingSlots)
		{
			Vector2 slotPosition = slot + ball.PositionInBoard;
			if (slotPosition.x >= 0 && slotPosition.x < BoardWidth)
			{
				BubbleDepthBallController ballInSlot = GetBallForPosition(slotPosition);
				if (ballInSlot != null)
				{
					stoundingBalls.Add(ballInSlot);
				}
			}
		}
		return stoundingBalls;
	}

	private List<BubbleDepthBallController> GetFloatingBalls()
	{
		foreach (BubbleDepthBallController b in _allBalls)
		{
			b.Checked = false;
		}

		List <BubbleDepthBallController> ballsToCheck = new List<BubbleDepthBallController>();
		List<BubbleDepthBallController> lowestRowBalls = GetLowestRowBalls();

		foreach (BubbleDepthBallController baseBall in lowestRowBalls)
		{
			ballsToCheck.Add(baseBall);
			do
			{
				ballsToCheck[0].Checked = true;
				List<BubbleDepthBallController> sroundingBalls = GetSroundingBalls(ballsToCheck[0]);
				ballsToCheck.RemoveAt(0);
				foreach (BubbleDepthBallController sroundingBall in sroundingBalls)
				{
					if (sroundingBall != null && !sroundingBall.Checked)
					{
						ballsToCheck.Add(sroundingBall);
						sroundingBall.Checked = true;
					}
				}
			} while(ballsToCheck.Count > 0);
		}

		List<BubbleDepthBallController> floatingBalls = new List<BubbleDepthBallController>();
		foreach (BubbleDepthBallController ball in _allBalls)
		{
			if (!ball.Checked)
			{
				floatingBalls.Add(ball);
			}
		}

		return floatingBalls;
	}

	private List<BubbleDepthBallController> GetLowestRowBalls()
	{
		List<BubbleDepthBallController> lowestRowBalls = new List<BubbleDepthBallController>();

		foreach (BubbleDepthBallController ball in _allBalls)
		{
			if (ball.PositionInBoard.y == _lowestRow)
			{
				lowestRowBalls.Add(ball);
			}
		}
		return lowestRowBalls;
	}

	IEnumerator DestroyBombs(GameObject firstBomb)
	{
		_pauseDecending = true;
		int firstBombIndex = _allBombs.IndexOf(firstBomb);


		firstBomb.GetComponent<BubbleDepthBombController>().ExplodeBomb();
		yield return new WaitForEndOfFrame();

		int indexCount = 0;

		bool finishd = true;
		do
		{
			indexCount++;

			finishd = true;

			if (firstBombIndex + indexCount < _allBombs.Count)
			{
				GameObject rightBomb = _allBombs[firstBombIndex + indexCount];
				rightBomb.GetComponent<BubbleDepthBombController>().ExplodeBomb();
				finishd = false;
				yield return new WaitForSeconds(0.1f);
			}

			if (firstBombIndex - indexCount >= 0)
			{
				GameObject leftBomb = _allBombs[firstBombIndex - indexCount];
				leftBomb.GetComponent<BubbleDepthBombController>().ExplodeBomb();
				finishd = false;
				yield return new WaitForSeconds(0.1f);
			}


		} while (!finishd);

		yield return new WaitForSeconds(0.1f);

		if (OnLevelComplete != null)
		{
			OnLevelComplete();
		}
	}

	#endregion


	#region Events

	private void OnBallHit(BubbleDepthBallController ball, BubbleDepthBallController hit)
	{
		PositionNewBall(ball, hit);
		
		ball.OnBallHit -= OnBallHit;
		
		StartCoroutine(CalculateHit(ball));
	}
	
	private void OnBallHitSinkingObject(BubbleDepthBallController ball)
	{
		DestroyBallController(ball);
		if (OnLevelComplete != null)
		{
			OnLevelComplete();
		}
	}

	private void OnBallHitBomb(BubbleDepthBallController ballController, GameObject bomb)
	{
		Debug.Log("hit bomb");
		DestroyBallController(ballController);
		StartCoroutine(DestroyBombs(bomb));
	}

	#endregion
}
