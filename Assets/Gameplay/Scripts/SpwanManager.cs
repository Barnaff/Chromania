using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ChromieSpwanedDelegate(ChromieController chromie);

public class SpwanManager : MonoBehaviour {
	
	#region Public Properties

	public ChromieDroppedDelegate OnChromieDropped;

	public ChromieCollectedDelegate OnChromieCollected;

    public ChromieSpwanedDelegate OnChromieSpwaned;

    #endregion

    #region Public Properties

    /// <summary>
    /// The chromiez prefabs.
    /// </summary>
    public GameObject[] ChromiezPrefabs;

	/// <summary>
	/// The _selected colors.
	/// </summary>
	private ColorType[] _selectedColors;

	private GameData _gameData;

	#endregion


	private enum SpwanerPhase
	{
		None,
		WaveStarted,
		SequanceStarted,
		SpwanItems,
		SequenceFinished,
		WaveFinished,
	}
 

	#region Private

	private SpwanerPhase _phase;

	private List<ChromieController> _chromiezPool = new List<ChromieController>();

	private List<WaveDefenition> _wavesList;

	private int _currentLevel;

	private WaveDefenition _currentWave;

	private SequanceDefenition _currentSequance;

	private float _waveTimeCount;

	private float _sequanceTimeCount;

	private int _currentSequanceIndex;
	
	private bool _paused;

	private Vector3 _spwanBasePosition;

    private ColorType _overrideColor = ColorType.None;

    #endregion


    #region Initialize

    // Use this for initialization
    void Start () {

		_spwanBasePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, 0, 10));

		IGameSetup gameSetupmanager = ComponentsFactory.GetAComponent<IGameSetup>() as IGameSetup;
		_selectedColors = gameSetupmanager.SelectedColors;

		_wavesList = WavesDataLoader.WavesList();

		IGameDataLoader gameDataLoader = ComponentsFactory.GetAComponent<IGameDataLoader>() as IGameDataLoader;
		if (gameDataLoader != null)
		{
			_gameData = gameDataLoader.GetGameData();
		}

		_phase = SpwanerPhase.None;

		Paused = true;
	}

	#endregion


	#region Update
	
	// Update is called once per frame
	void Update () {
		if (!Paused)
		{
			switch (_phase)
			{
			case SpwanerPhase.None:
			{
				StartNewWave();
				break;
			}
			case SpwanerPhase.WaveStarted:
			{
				_waveTimeCount += Time.deltaTime;
				if (_waveTimeCount > _currentWave.StartDelay)
				{
					StartNextSequance();
				}
				break;
			}
			case SpwanerPhase.SequanceStarted:
			{
				_sequanceTimeCount += Time.deltaTime;
				if (_sequanceTimeCount > _currentSequance.StartInterval)
				{
					_phase = SpwanerPhase.SpwanItems;
				}
				break;
			}
			case SpwanerPhase.SpwanItems:
			{
				SpwanItems();
				break;
			}
			case SpwanerPhase.SequenceFinished:
			{
				_sequanceTimeCount += Time.deltaTime;
				if (_sequanceTimeCount > _currentSequance.EndInterval)
				{
					EndSequance();
				}
				break;
			}
			case SpwanerPhase.WaveFinished:
			{
				_waveTimeCount += Time.deltaTime;
				if (_waveTimeCount > _currentWave.EndDelay)
				{
					_phase = SpwanerPhase.None;
				}
				break;
			}
			default:
			{
				_phase = SpwanerPhase.None;
				break;
			}
			}
		}
	}


    #endregion


    #region Public

    public bool Paused
    {
        get
        {
            return _paused;
        }

        set
        {
            _paused = value;
        }
    }

    public void MakeAllSpwansSameColor(ColorType color)
    {
        _overrideColor = color;
    }

    public void ReturnToOriginalSelectedColors()
    {
        _overrideColor = ColorType.None;
    }

    #endregion


    #region Private

    private void StartNewWave()
	{
		_currentWave = GetNewWave();
		_waveTimeCount = 0;
		_currentSequanceIndex = 0;
		_phase = SpwanerPhase.WaveStarted;
	}

	private void StartNextSequance()
	{
		if (_currentWave.SequanceList.Count > _currentSequanceIndex)
		{
			_currentSequance = _currentWave.SequanceList[_currentSequanceIndex];
			_currentSequanceIndex++;
			_sequanceTimeCount = 0;
			_phase = SpwanerPhase.SequanceStarted;
		}
		else
		{
			_phase = SpwanerPhase.WaveFinished;
		}
	}

	private void EndSequance()
	{
		StartNextSequance();
	}

	private void SpwanItems()
	{
		foreach (SpwanedItemDefenition spwanItem in _currentSequance.SpwanItemList)
		{
			SpwanItem(spwanItem);
		}
		_sequanceTimeCount = 0;
		_phase = SpwanerPhase.SequenceFinished;
	}

	private WaveDefenition GetNewWave()
	{
		WaveDefenition wave = _wavesList[Random.Range(0,_wavesList.Count)];
		return wave;
	}

	private void SpwanItem(SpwanedItemDefenition spwanedItem)
	{
        ColorType colorType;
        if (_overrideColor != ColorType.None)
        {
            colorType = _overrideColor;
        }
        else
        {
            colorType = ColorForSpwanColorType(spwanedItem.SpwanedColor);
        }
		GameObject chromie = CreateChromie(colorType);

		Vector3 spwanPosition = _spwanBasePosition;
		spwanPosition.x += spwanedItem.XPosition * 0.032f;
		chromie.transform.position = spwanPosition;

		Vector2 spwanForce = spwanedItem.ForceVector;
		spwanForce.y += 450;
		chromie.GetComponent<Rigidbody2D>().AddForce(spwanForce);
		chromie.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-90, 90);

        if (OnChromieSpwaned != null)
        {
            OnChromieSpwaned(chromie.GetComponent<ChromieController>());
        }
	}

	private ColorType ColorForSpwanColorType(SpwanedColorType spwanColorType)
	{
		ColorType colorType = ColorType.None;
		switch (spwanColorType)
		{
		case SpwanedColorType.RandomCorner:
		{
			colorType = _selectedColors[Random.Range(0,_selectedColors.Length)];
			break;
		}
		case SpwanedColorType.BottomLeft:
		{
			colorType = _selectedColors[3];
			break;
		}
		case SpwanedColorType.TopLeft:
		{
			colorType = _selectedColors[0];
			break;
		}
		case SpwanedColorType.TopRight:
		{
			colorType = _selectedColors[1];
			break;
		}
		case SpwanedColorType.BottomRight:
		{
			colorType = _selectedColors[2];
			break;
		}
		}
		return colorType;
	}

	#endregion


	#region Create Chromiez

	private GameObject CreateChromie(ColorType colorType)
	{
		foreach (ChromieController chromieInPool in _chromiezPool)
		{
			if (!chromieInPool.gameObject.activeInHierarchy && chromieInPool.ChromieType == colorType)
			{
				chromieInPool.gameObject.SetActive(true);
				chromieInPool.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				_chromiezPool.Remove(chromieInPool);
				return chromieInPool.gameObject;
			}
		}

		ChromieDataItem chromieData = _gameData.GetChromie(colorType);

		GameObject newChromie = Instantiate(ChromiePrefabForColorType(colorType)) as GameObject;
		ChromieController chromieController = newChromie.GetComponent<ChromieController>();
		if (chromieController != null)
		{
			chromieController.OnChromieRemoved += OnChromieRemovedHandler;
			chromieController.OnChromieDropped += OnChromieDroppedHandler;
			chromieController.OnChromieCollected += OnChromieCollectedHandler;
			chromieController.ChromieData = chromieData;
		}
		ChromiePowerupController chromiePowerupController = newChromie.GetComponent<ChromiePowerupController>() as ChromiePowerupController;
		if (chromiePowerupController != null)
		{
			chromiePowerupController.ChromieData = chromieData;
        }

		return newChromie;
	}

	private GameObject ChromiePrefabForColorType(ColorType colorType)
	{
		foreach (GameObject chromiePrefab in ChromiezPrefabs)
		{
			if (chromiePrefab.GetComponent<ChromieController>().ChromieType == colorType)
			{
				return chromiePrefab;
			}
		}
		return null;
	}

	private void RemoveCrhomie(ChromieController chromieController)
	{
		chromieController.gameObject.SetActive(false);
		if (!_chromiezPool.Contains(chromieController))
		{
			_chromiezPool.Add(chromieController);
		}
	}

	#endregion


	#region Events

	private void OnChromieCollectedHandler(ChromieController chromieController)
	{
		if (OnChromieCollected != null)
		{
			OnChromieCollected(chromieController);
		}
		RemoveCrhomie(chromieController);
	}

	private void OnChromieDroppedHandler(ChromieController chromieController)
	{
		if (OnChromieDropped != null)
		{
			OnChromieDropped(chromieController);
		}
		RemoveCrhomie(chromieController);
	}

	private void OnChromieRemovedHandler(ChromieController chromieController)
	{
		RemoveCrhomie(chromieController);
	}

	#endregion
}
