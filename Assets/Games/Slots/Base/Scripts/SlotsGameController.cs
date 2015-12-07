using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SlotsGameController : GameplayBase {
	

	[SerializeField]
	private GameObject _slotsWindow;

	[SerializeField]
	private GameObject _slotsCanvas;
	
	[SerializeField]
	private Camera _gameCamera;

	[SerializeField]
	private GameObject[] _reelsContainers;

	[SerializeField]
	private SlotsButtonsController _buttonsController;

	[SerializeField]
	private SlotsPayTablePopupController _payoutPopupController;


	[SerializeField]
	private GameObject _scatterPanel;

	[SerializeField]
	private Text _scatterLabel;

	[SerializeField]
	private Sprite[] _iconsSprites;


	private Vector2 _tileSize;
	
	private Vector2 _slotsWindowSize;

	private bool _isSpinning;

	private List<GameObject> _lastTiles;

	private GameObject _linesContainer;

	private List<GameObject> _lines;

	private List<Vector3> _tilesAnchors; 

	private bool _isBlinkingLines;

	private int _linesCount = 1;
	private int _betCount = 1;

	private float _totalCount = 0;


	private int _scatterCount = 0;

	[SerializeField]
	private Shader _lineShader;

	[SerializeField]
	private SlotsDefenition _slotsDefenition;


	// simulations

	private bool _isSimulating;

	private SlotsSimulationResults _simulationsResults;

	private float _simulationProgress;


	#region Gameplaybase Implementation

	protected override void GameStarted (string gameVariables)
	{
		base.GameStarted (gameVariables);

		if (!string.IsNullOrEmpty(gameVariables))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			
			SurrogateSelector surrogateSelector = new SurrogateSelector();
			
			ColorSurrogate colorSurrogate = new ColorSurrogate();
			surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All) , colorSurrogate);
			
			SlotsIconDefenitionSurrogate slotsIconDefenitionSurrogate = new SlotsIconDefenitionSurrogate();
			surrogateSelector.AddSurrogate(typeof(SlotsIconDefenition), new StreamingContext(StreamingContextStates.All) , slotsIconDefenitionSurrogate);
			
			binaryFormatter.SurrogateSelector = surrogateSelector;

			SlotsDefenition slotsDefenition = (SlotsDefenition)ObjectSerializerUtil.Deserialize(gameVariables, binaryFormatter);
			_slotsDefenition = slotsDefenition;
			
			foreach (SlotsIconDefenition icon in _slotsDefenition.IconsDefenitions)
			{
				if (!string.IsNullOrEmpty(icon.IconSpriteName))
				{
					Sprite sprite = GetSpritePrfab(icon.IconSpriteName);
					icon.IconSprite = sprite;
				}
			}
		}
		StartCoroutine(GenerateReels());
	}

	#endregion


	#region Public

	public SlotsDefenition GameDefenition
	{
		get
		{
			return _slotsDefenition;
		}
		set
		{
			_slotsDefenition = value;
		}
	}

	#endregion


	#region Private

	protected override void Init()
	{
		Rect slotsWindowRect = RectTransformUtility.PixelAdjustRect(_slotsWindow.GetComponent<RectTransform>(), _slotsCanvas.GetComponent<Canvas>());
		_slotsWindowSize = new Vector2(slotsWindowRect.width, slotsWindowRect.height);
		_tileSize = new Vector2(_slotsWindowSize.x / _slotsDefenition.NumberOfReels, _slotsWindowSize.y / 3.0f);

		_gameCamera.orthographicSize = Screen.height * 0.5f;

		_tilesAnchors = new List<Vector3>();

		if (PlayerPrefsUtil.HasKey("betCount"))
		{
			_betCount = PlayerPrefsUtil.GetInt("betCount");

			if (_betCount > _slotsDefenition.SlotsBetAmounts.Length)
			{
				_betCount = _slotsDefenition.SlotsBetAmounts.Length;
			}
		}

		if (PlayerPrefsUtil.HasKey("linesCount"))
		{
			_linesCount = PlayerPrefsUtil.GetInt("linesCount");

			if (_linesCount > _slotsDefenition.Lines.Length)
			{
				_linesCount = _slotsDefenition.Lines.Length;
			}
		}

		UpdateTotalCount();


		if (_buttonsController != null)
		{
			_buttonsController.OnSpin += HandleOnSpin;
			_buttonsController.OnMaxBet += HandleOnMaxBet;
			_buttonsController.OnAddLine += HandleOnAddLine;
			_buttonsController.OnReduceLine += HandleOnReduceLine;
			_buttonsController.OnAddBet += HandleOnAddBet;
			_buttonsController.OnReduceBet += HandleOnReduceBet;
			_buttonsController.OnPayTable += HandleOnPayTable;
			_buttonsController.SetLines(_linesCount);
			_buttonsController.SetBet(_slotsDefenition.SlotsBetAmounts[_betCount]);
		}


		for (int i=0; i< _slotsDefenition.IconsDefenitions.Length; i++)
		{
			SlotsIconDefenition iconDefenition = _slotsDefenition.IconsDefenitions[i];
			iconDefenition.IconSprite = GetSpritePrfab(iconDefenition.IconSpriteName);
		}

		_scatterPanel.SetActive(false);
	}


	IEnumerator GenerateReels()
	{
		List<GameObject> firstTiles = new List<GameObject>();
		_lastTiles = new List<GameObject>();
		yield return null;
		for (int x = 0 ; x < _reelsContainers.Length; x++)
		{
			int numberOfTiles = _slotsDefenition.TilesPerReel + (x * _slotsDefenition.TilesPerReelDifirance);
			RemoveAllChildren(_reelsContainers[x].transform);


			for (int y = 0 ; y < numberOfTiles; y++)
			{
				GameObject newTile = new GameObject();
				newTile.transform.SetParent(_reelsContainers[x].transform);
				newTile.name = "tile " + x + ":" + y;
				newTile.transform.localScale = new Vector3(1,1,1);
				LayoutElement layoutElement = newTile.AddComponent<LayoutElement>();
				layoutElement.preferredWidth = _tileSize.x;
				layoutElement.preferredHeight = _tileSize.y;

				SlotsIconDefenition iconDefenition = GetRandomIcon();

				SlotsTileController tileController = newTile.AddComponent<SlotsTileController>();
				tileController.IconDefenition = iconDefenition;

				Image tileImage = newTile.AddComponent<Image>();
				Sprite iconSprite = iconDefenition.IconSprite;
				tileImage.sprite = iconSprite;

				if (y <  3)
				{
					_lastTiles.Add(newTile);
				}

				if (y >= numberOfTiles - 3)
				{
					firstTiles.Add(newTile);
				}

			}

		}

		yield return null;
		for (int i = 0 ; i < firstTiles.Count; i++)
		{
			Vector3 tilePosition = firstTiles[i].transform.position;
			_tilesAnchors.Add(tilePosition);
		}
		DrawLines();

		HideAllLines();

	}

	private void ShuffleTiles()
	{
		for (int x = 0 ; x < _reelsContainers.Length; x++)
		{
			for (int y=0; y < _reelsContainers[x].transform.childCount; y++)
			{
				GameObject tile = _reelsContainers[x].transform.GetChild(y).gameObject;

				if (tile != null)
				{
					SlotsTileController tileController = tile.GetComponent<SlotsTileController>();
					if (tileController != null)
					{
						//int randomIconId = Random.Range(0, _slotsDefenition.IconsDefenitions.Length);
						SlotsIconDefenition iconDefenition = GetRandomIcon();
						tileController.IconDefenition = iconDefenition;

						Sprite iconSprite = iconDefenition.IconSprite;
						tile.GetComponent<Image>().sprite = iconSprite;
					}
				}
			}
		}
	}

	private void Spin()
	{
		if (_isSpinning)
		{
			return;
		}

		_isSpinning = true;
		PlaySoundFX("Reel_Spin_Long");
		HideAllLines();

		ShuffleTiles();

		_buttonsController.SetPanelItemsState(false);

		for (int i=0; i< _reelsContainers.Length; i++)
		{
			int reelIndex = i;

			RectTransform reelRectTransform = _reelsContainers[reelIndex].GetComponent<RectTransform>();
			reelRectTransform.pivot = new Vector2(0.5f, 0);

			int numberOfTiles = _slotsDefenition.TilesPerReel + (reelIndex * _slotsDefenition.TilesPerReelDifirance);


			System.Action <float> updateAction = (value)=>
			{
				reelRectTransform.pivot = new Vector2(0.5f, (1f + ((1f / numberOfTiles))) * value);
			};

			System.Action <float> bouneUpdate = (value)=>
			{
				float reelPositionOffset = Mathf.Lerp(reelRectTransform.pivot.y, 1.0f, 30.0f * Time.deltaTime);
				reelRectTransform.pivot = new Vector2(0.5f, reelPositionOffset);
			};

			System.Action finishAction = ()=>
			{
				reelRectTransform.pivot = new Vector2(0.5f, 1);
				if (reelIndex == _reelsContainers.Length -1)
				{
					StartCoroutine(FinishedSpinning());
				}
			};

			System.Action bounceAction = ()=>
			{
				if (reelIndex == 0)
				{
					StopSoundFX();
				}
				PlaySoundFX("Reel_Stop4", null, false, 0, float.MaxValue, 1.0f +  (reelIndex * 0.05f) );
				iTween.ValueTo(reelRectTransform.gameObject, iTween.Hash("time", 0.5f , "from", 0f, "to", 1f, "onupdateAction", bouneUpdate, "oncompleteaction", finishAction));

			};

			iTween.ValueTo(reelRectTransform.gameObject, iTween.Hash("time", 0.2f + (numberOfTiles * 0.05f) , "from", 0f, "to", 1f, "onupdateAction", updateAction, "oncompleteaction", bounceAction));
		}


	}

	IEnumerator FinishedSpinning()
	{


		_isSpinning = false;

		List<SlotsWinningsInfo> winningInfoList = GetWinnings();

		List<int> blinkingLinesList = new List<int>();

		foreach (SlotsWinningsInfo winningInfo in winningInfoList)
		{
			if (winningInfo.isWinning)
			{
				foreach (SlotsTileController tile in winningInfo.Tiles)
				{
					tile.DisplayTileWinningAnimation();
				}

				if (!blinkingLinesList.Contains(winningInfo.LineIndex))
				{
					blinkingLinesList.Add(winningInfo.LineIndex);
				}

				if (winningInfo.Icon.IconBehaviorType == SlotsIconDefenition.SlotsIconBehaviorType.Scatter)
				{
					_scatterCount++;
				}
			}
		}

		if (blinkingLinesList.Count > 0)
		{
			PlaySoundFX("Line");
			if (blinkingLinesList.Count > 2)
			{
				PlaySoundFX("Small_Win1");
			}

			StartCoroutine(BlinkLines(blinkingLinesList));
		}

		PayOut(winningInfoList);

		yield return new WaitForSeconds(1.0f);

		if (_scatterCount > 0)
		{
			StartCoroutine( StartScatter());
		}
		else
		{
			_scatterPanel.SetActive(false);
			_buttonsController.SetPanelItemsState(true);
		}


	}

	private void RemoveAllChildren(Transform transform)
	{
		for (int i=0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	IEnumerator StartScatter()
	{
		if (_scatterCount > 0)
		{
			_scatterPanel.SetActive(true);
			_scatterLabel.text = _scatterCount.ToString();

			yield return new WaitForSeconds(0.5f);

			_scatterCount--;
			_scatterLabel.text = _scatterCount.ToString();

			yield return new WaitForSeconds(0.5f);

			Spin();
		}
	}


	private void DrawLines()
	{
		_lines = new List<GameObject>();
		_linesContainer = new GameObject();
		_linesContainer.transform.position = Vector3.zero;
		_linesContainer.name = "Lines Container";
		for (int i=0; i< _slotsDefenition.Lines.Length; i++)
		{
			SlotsLineDefenition line = _slotsDefenition.Lines[i];
			DrawLine(line, i);
		}

	}

	private void DrawLine(SlotsLineDefenition line, int lineIndex)
	{
		GameObject lineContainer = new GameObject();
		lineContainer.transform.SetParent(_linesContainer.transform);
		lineContainer.name = "Line " + lineIndex;

		Vector3 firstAnchor = _tilesAnchors[(int)_slotsDefenition.Lines[lineIndex].AnchorPositions[0]];
		firstAnchor.x -= _tileSize.x * 0.5f;
		firstAnchor.z = 50;

		Vector3 lastAnchor = _tilesAnchors[((_slotsDefenition.NumberOfReels -1) * 3) + (int)_slotsDefenition.Lines[lineIndex].AnchorPositions[_slotsDefenition.Lines[lineIndex].AnchorPositions.Length -1]];
		lastAnchor.x += _tileSize.x * 0.5f;
		lastAnchor.z = 50;

		Vector3 previusAnchor = firstAnchor;

		for (int x = 0; x < _slotsDefenition.NumberOfReels + 1; x++)
		{
			GameObject lineSegment = new GameObject();
			lineSegment.transform.SetParent(lineContainer.transform);
			LineRenderer lineRenderer = lineSegment.AddComponent<LineRenderer>();
			lineRenderer.SetVertexCount(2);
			
			lineRenderer.material = new Material(_lineShader);
			lineRenderer.material.color = line.LineColor;

			float lineWidth = Screen.width * 0.01f;
			lineRenderer.SetWidth(lineWidth, lineWidth);

			if (x < _slotsDefenition.NumberOfReels)
			{
				int y = (int)line.AnchorPositions[x];
				lineSegment.name = "line segment " + x + ":" + y;
				Vector3 anchorPosition = _tilesAnchors[(x * 3) + y];
				anchorPosition.z = 50;

				anchorPosition += Random.insideUnitSphere * Screen.width * 0.01f;

				lineRenderer.SetPosition(0, previusAnchor);
				lineRenderer.SetPosition(1, anchorPosition);

				previusAnchor = anchorPosition;
			}
			else
			{
				lineRenderer.SetPosition(0, previusAnchor);
				lineRenderer.SetPosition(1, lastAnchor);
			}
		}
		_lines.Add(lineContainer);

	}

	private void ShowAllLines()
	{
		HideAllLines();

		for (int i=0; i< _linesCount; i++)
		{
			_lines[i].SetActive(true);
		}
	}

	private void HideAllLines()
	{
		StopCoroutine(BlinkLines(null));
		_isBlinkingLines = false;
		for (int i=0; i< _lines.Count; i++)
		{
			_lines[i].SetActive(false);
		}
	}

	IEnumerator BlinkLines(List<int> lines)
	{
		_isBlinkingLines = true;
		bool isShown = true;
		while(_isBlinkingLines)
		{
			for (int i=0; i< lines.Count; i++)
			{
				_lines[lines[i]].SetActive(isShown);
			}
			isShown = !isShown;
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void ToggleLineDisplay(int lineIndex, bool displayState)
	{
		GameObject line = _lines[lineIndex];
		line.SetActive(displayState);
	}


	private List<SlotsWinningsInfo> GetWinnings()
	{
		List<SlotsWinningsInfo> winnings = new List<SlotsWinningsInfo>();

		for (int i =0; i< _linesCount; i++)
		{

			int jokerCount = 0;
			List<SlotsTileController> jokerTiles = new List<SlotsTileController>();

			SlotsLineDefenition line = _slotsDefenition.Lines[i];

			List<GameObject> tilesOnLine = new List<GameObject>();

			Dictionary<SlotsIconDefenition,int> lineResults = new Dictionary<SlotsIconDefenition, int>();

			for (int x = 0; x < line.AnchorPositions.Length; x++)
			{
				int tileIndex = (x * 3) + (int)line.AnchorPositions[x];

				GameObject tile = _lastTiles[tileIndex];

				SlotsTileController tileController = tile.GetComponent<SlotsTileController>();

				if (lineResults.ContainsKey(tileController.IconDefenition))
				{
					lineResults[tileController.IconDefenition] = lineResults[tileController.IconDefenition] + 1;
				}
				else
				{
					lineResults.Add(tileController.IconDefenition, 1);
				}

				if (tileController.IconDefenition.IconBehaviorType == SlotsIconDefenition.SlotsIconBehaviorType.Joker)
				{
					jokerCount++;
					jokerTiles.Add(tileController);
				}

				tilesOnLine.Add(tile);
			}

			foreach (SlotsIconDefenition iconDefenitionKey in lineResults.Keys)
			{
				SlotsWinningsInfo winningInfo = new SlotsWinningsInfo();
				winningInfo.LineIndex = i;
				winningInfo.isWinning = false;
				winningInfo.JokerCount = jokerCount;

				if (lineResults[iconDefenitionKey] + jokerCount >= 3)
				{
					winningInfo.Icon = iconDefenitionKey;
					winningInfo.Count = lineResults[iconDefenitionKey];

					winningInfo.isWinning = true;


					foreach (GameObject tile in tilesOnLine)
					{
						if (tile.GetComponent<SlotsTileController>().IconDefenition == iconDefenitionKey)
						{
							winningInfo.Tiles.Add(tile.GetComponent<SlotsTileController>());
						}
					}

					if (jokerTiles.Count > 0)
					{
						winningInfo.Tiles.AddRange(jokerTiles);
					}
				}

				if (winningInfo.isWinning)
				{
					winnings.Add(winningInfo);
				}
			}
		}
	

		return winnings;
	}

	private SlotsIconDefenition GetRandomIcon()
	{
		float chanceValue = Random.Range(0,100);
		SlotsIconDefenition.SlotsIconRarityType rarity = SlotsIconDefenition.SlotsIconRarityType.Common;
		if (chanceValue < 10)
		{
			rarity = SlotsIconDefenition.SlotsIconRarityType.Mythic;
		}
		else if (chanceValue < 25)
		{
			rarity = SlotsIconDefenition.SlotsIconRarityType.Rare;
		}
		else if (chanceValue < 50)
		{
			rarity = SlotsIconDefenition.SlotsIconRarityType.Uncommon;
		}
		else
		{
			rarity = SlotsIconDefenition.SlotsIconRarityType.Common;
		}

		SlotsIconDefenition randomIconDefenition = null;
		List<SlotsIconDefenition> iconsForRarity = new List<SlotsIconDefenition>();
		foreach (SlotsIconDefenition iconDefenition in _slotsDefenition.IconsDefenitions)
		{
			if (iconDefenition.IconRarity == rarity)
			{
				iconsForRarity.Add(iconDefenition);
			}
		}

		if (iconsForRarity.Count > 0)
		{
			randomIconDefenition = iconsForRarity[Random.Range(0, iconsForRarity.Count)];
		}

		if (randomIconDefenition == null)
		{
			randomIconDefenition = GetRandomIcon();
		}
		return randomIconDefenition;
	}

	private void UpdateTotalCount()
	{
		if (_betCount >= _slotsDefenition.SlotsBetAmounts.Length || _betCount < 0)
		{
			_betCount = 0;
		}
		_totalCount = _linesCount * _slotsDefenition.SlotsBetAmounts[_betCount];
		if (_buttonsController != null)
		{
			_buttonsController.SetTotal(_totalCount);
		}

//		bool canPay = CanPayAmount((int)_totalCount);
//		if (_buttonsController != null)
//		{
//			_buttonsController.SetPanelSpinEnabled(canPay);
//		}
	}

	public void PayOut(List<SlotsWinningsInfo> winningsList)
	{
		float winAmount = 0;

		foreach (SlotsWinningsInfo winning in winningsList)
		{
			float multiplier = 1.0f;
			if (winning.isWinning)
			{

				switch (winning.Count)
				{
				case 4:
				{
					multiplier = winning.Icon.BetMultiplierFor4;
					break;
				}
				case 5: 
				{
					multiplier = winning.Icon.BetMultiplierFor5;
					break;
				}
				default:
				{
					multiplier = winning.Icon.BetMultiplierFor3;
					break;
				}
				}
			}
			float betAmount = _slotsDefenition.SlotsBetAmounts[_betCount];
			winAmount += (betAmount * multiplier);
		}

		if (_buttonsController != null)
		{
			_buttonsController.SetWin(winAmount, (winAmount > 0) ? true : false);
		}

		AddCoins((int)winAmount);
	}

	public Sprite GetSpritePrfab(string spriteName)
	{
		foreach (Sprite sprite in _iconsSprites)
		{
			if (sprite.name == spriteName)
			{
				return sprite;
			}
		}
		Debug.LogError("ERROR - Missing sprite for : " + spriteName);
		return null;
	}

	#endregion


	#region User Interactions

	void HandleOnReduceBet ()
	{
		_betCount--;
		if (_betCount < 0)
		{
			_betCount = _slotsDefenition.SlotsBetAmounts.Length - 1;
		}
		PlayerPrefsUtil.SetInt("betCount", _betCount);
		_buttonsController.SetBet(_slotsDefenition.SlotsBetAmounts[_betCount]);
		UpdateTotalCount();
	}
	
	void HandleOnAddBet ()
	{
		_betCount ++;
		if (_betCount >=  _slotsDefenition.SlotsBetAmounts.Length)
		{
			_betCount = 0;
		}
		PlayerPrefsUtil.SetInt("betCount", _betCount);
		_buttonsController.SetBet(_slotsDefenition.SlotsBetAmounts[_betCount]);
		UpdateTotalCount();
	}
	
	void HandleOnReduceLine ()
	{
		_linesCount--;
		if (_linesCount < 1)
		{
			_linesCount = _slotsDefenition.Lines.Length;
		}
		PlayerPrefsUtil.SetInt("linesCount", _linesCount);
		_buttonsController.SetLines(_linesCount);

		ShowAllLines();
		UpdateTotalCount();
	}
	
	void HandleOnAddLine ()
	{
		_linesCount++;
		if (_linesCount > _slotsDefenition.Lines.Length)
		{
			_linesCount = 1;
		}
		PlayerPrefsUtil.SetInt("linesCount", _linesCount);
		_buttonsController.SetLines(_linesCount);

		ShowAllLines();
		UpdateTotalCount();
	}
	
	void HandleOnMaxBet ()
	{
		_linesCount = _slotsDefenition.Lines.Length;
		_betCount = _slotsDefenition.SlotsBetAmounts.Length - 1;

		_buttonsController.SetLines(_linesCount);
		_buttonsController.SetBet(_slotsDefenition.SlotsBetAmounts[_betCount]);

		ShowAllLines();
		UpdateTotalCount();

	}
	
	void HandleOnSpin ()
	{
		if (PayCoins((int)_totalCount))
		{
			_buttonsController.SetWin(0,false);
			Spin();
		}
	}

	void HandleOnPayTable ()
	{
		_payoutPopupController.DisplayPayout(_slotsDefenition);
	}


	#endregion


	#region Simulations

	[ExecuteInEditMode]
	public void StartSimulation(int numberOfSpins)
	{
		StartCoroutine(SimulationCorutine(numberOfSpins));
	}

	[ExecuteInEditMode]
	IEnumerator SimulationCorutine(int numberOfSpins)
	{
		_isSimulating = true;
		_simulationProgress = 0;
		List<SlotsWinningsInfo> winningInfoList = null;
		_simulationsResults = new SlotsSimulationResults();
		float betAmount = _slotsDefenition.SlotsBetAmounts[_betCount];
		int simulationCount = 0;
		int scatterCount = 0;
		while (simulationCount < numberOfSpins && _isSimulating)
		{
			simulationCount++;
			ShuffleTiles();
			yield return null;
			winningInfoList = GetWinnings();

			if (scatterCount > 0)
			{
				scatterCount--;
				_simulationsResults.NumberOfScatters++;
				_simulationsResults.SavedByScatter += (int)_totalCount;
			}
			else
			{
				_simulationsResults.CoinsBalance -= (int)_totalCount;
				_simulationsResults.PayAmount += (int)_totalCount;
			}
		
			_simulationsResults.SpinsCounted++;
			bool haveJoker = false;
			if (winningInfoList.Count > 0)
			{
				_simulationsResults.WinCount++;
			}

			foreach (SlotsWinningsInfo winning in winningInfoList)
			{
				float multiplier = 1.0f;
				if (winning.isWinning)
				{

					SlotsSimulationIconInfo iconInfo = null;
					if (_simulationsResults.IconsInfo.ContainsKey(winning.Icon.IconSpriteName))
					{
						iconInfo = _simulationsResults.IconsInfo[winning.Icon.IconSpriteName];
					}
					else
					{
						iconInfo = new SlotsSimulationIconInfo();
						_simulationsResults.IconsInfo.Add(winning.Icon.IconSpriteName, iconInfo);
					}
					
					iconInfo.Count++;


					switch (winning.Count)
					{
					case 4:
					{
						multiplier = winning.Icon.BetMultiplierFor4;
						_simulationsResults.Forths++;
						iconInfo.Forths++;
						break;
					}
					case 5: 
					{
						multiplier = winning.Icon.BetMultiplierFor5;
						_simulationsResults.Fifths++;
						iconInfo.Fifths++;
						break;
					}
					default:
					{
						multiplier = winning.Icon.BetMultiplierFor3;
						_simulationsResults.Tripples++;
						iconInfo.Trippels++;
						break;
					}
					}


					if (winning.Icon.IconBehaviorType == SlotsIconDefenition.SlotsIconBehaviorType.Scatter)
					{
						scatterCount += winning.Count;
					}

					int winAmount = (int)(betAmount * multiplier);
					_simulationsResults.WinAmount += winAmount;
					_simulationsResults.CoinsBalance += winAmount;
					iconInfo.TotalWin += winAmount;

					if (_simulationsResults.MaxWin < winAmount)
					{
						_simulationsResults.MaxWin = winAmount;
					}

					iconInfo.Percentage = (float)iconInfo.TotalWin / (float)_simulationsResults.WinAmount;

					if (winning.JokerCount > 0)
					{
						haveJoker = true;
					}


				}
			}
			if (haveJoker)
			{
				_simulationsResults.JokerCount++;
			}
			_simulationProgress = (float)simulationCount / (float)numberOfSpins;


		}
		Debug.Log("finished simulation");
		yield return null;

	}

	public void StopSimulation()
	{
		_isSimulating = false;
	}

	public bool IsSimulating
	{
		get
		{
			return _isSimulating;
		}
	}

	public float SimulationProgress
	{
		get
		{
			return _simulationProgress;
		}
	}

	public SlotsSimulationResults SimulationResults
	{
		get
		{
			return _simulationsResults;
		}
	}

	#endregion
}
