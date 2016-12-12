using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpwanerController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private ChromieController _chromieControllerPrefab;

    [SerializeField]
    private Vector3 _spwanBasePosition;

    private const float FORCE_VECTOR_MODIFIER = 450;
    private const float SPWAN_POSITION_MULTIPLIER = 0.032f;

    [SerializeField]
    private WavesData _wavesData;

    private SequanceDataObject _currentSequance = null;

    private WaveDataObject _currentWave = null;

    [SerializeField]
    private bool _isPaued;

    private List<SequanceDataObject> _loadedSequances;

    private List<SequanceDataObject> _usedSequances = new List<SequanceDataObject>();

    private float _waveTimeCount = 0;

    [SerializeField]
    private eChromieType _overrideColor;

    private eChromieType[] _selectedChromies;

    private eGameplayMode _currentGameMode;

    private int _currentLevel;

    private List<ChromieController> _allChromiez = new List<ChromieController>();

    private int _currentWaveIndex = 0;

    #endregion

#if UNITY_EDITOR

    public void SetWavesDataForEditMode(WavesData wavesData)
    {
        _wavesData = wavesData;
    }

    private SequanceDataObject _sequanceDataObjectForEditor;

    public void PlaySequanceForEditMode(SequanceDataObject sequance)
    {
        _sequanceDataObjectForEditor = sequance;
    }

#endif


    #region Spwaning

    void Update()
    {
        if (!_isPaued)
        {
            if (_currentSequance == null)
            {
                _currentSequance = GetNextSequance();
            }
            else
            {
                if (_currentWave == null)
                {
                    _currentWave = GetNextWave();
                }
                else
                {
                    if (_waveTimeCount < _currentWave.Delay)
                    {
                        _waveTimeCount += Time.deltaTime;
                    }
                    else
                    {
                        foreach (SpawnedItemDataObject spawnedItem in _currentWave.SpawnedItems)
                        {
                            SpwanItem(spawnedItem);
                        }
                        _waveTimeCount = 0;
                        _currentWave = null;
                    }
                }
            }
        }
    }

    private SequanceDataObject GetNextSequance()
    {
        List<SequanceDataObject> avalableSequances = GetAvalableSequances();
        SequanceDataObject sequancse = avalableSequances[Random.Range(0, avalableSequances.Count)];
        _currentWaveIndex = 0;
        _waveTimeCount = 0;
        return sequancse;
    }

    private WaveDataObject GetNextWave()
    {
        if (_currentSequance != null && _currentSequance.Waves != null)
        {
            if (_currentSequance.Waves.Count > _currentWaveIndex)
            {
                WaveDataObject nextWave = _currentSequance.Waves[_currentWaveIndex];
                _currentWaveIndex++;
                return nextWave;
            }
        }
        _waveTimeCount = 0;
        _currentSequance = null;
        return null;
    }

    #endregion



    #region Public

    public void Init(eGameplayMode gameMode, List<ChromieDefenition> selectedColors)
    {
        _loadedSequances = _wavesData.SequancesList;

        _spwanBasePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, 0, 10));

        _selectedChromies = new eChromieType[selectedColors.Count];
        for (int i = 0; i < selectedColors.Count; i++)
        {
            _selectedChromies[i] = selectedColors[i].ChromieColor;
        }

        _currentGameMode = gameMode;
        _currentLevel = 0;
    }

    public void StartSpwaning()
    {
        _isPaued = false;
    }

    public void StopSpwaning()
    {
        _isPaued = true;
    }

    public bool Paused
    {
        get
        {
            return _isPaued;
        }

        set
        {
            _isPaued = value;
        }
    }

    public void SetSpwanColorOverride(eChromieType overrideColor)
    {
        _overrideColor = overrideColor;
    }

    #endregion


    #region Private

    private void SpwanItem(SpawnedItemDataObject spwanedItem)
    {
        eChromieType colorType;
        if (_overrideColor != eChromieType.None)
        {
            colorType = _overrideColor;
        }
        else
        {
            colorType = ColorForSpwanColorType(spwanedItem.SpwanedColor);
        }
        ChromieController chromieController = CreateChromie(colorType);

        if (!_allChromiez.Contains(chromieController))
        {
            _allChromiez.Add(chromieController);
        }

        Vector3 spwanPosition = _spwanBasePosition;
        spwanPosition.x += spwanedItem.XPosition * SPWAN_POSITION_MULTIPLIER;
        chromieController.gameObject.transform.position = spwanPosition;

        Vector2 spwanForce = spwanedItem.ForceVector;
        spwanForce.y += FORCE_VECTOR_MODIFIER * (GameplaySettings.Instance.GameSpeedMultiplier * 0.7f);
        chromieController.gameObject.GetComponent<Rigidbody2D>().AddForce(spwanForce);
        chromieController.gameObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-90, 90);

        GameplayEventsDispatcher.SendChromieSpwaned(chromieController);
    }

    private eChromieType ColorForSpwanColorType(eSpwanedColorType spwanColorType)
    {
        eChromieType colorType = eChromieType.None;
        switch (spwanColorType)
        {
            case eSpwanedColorType.RandomCorner:
                {
                    colorType = _selectedChromies[Random.Range(0, _selectedChromies.Length)];
                    break;
                }
            case eSpwanedColorType.BottomLeft:
                {
                    colorType = _selectedChromies[3];
                    break;
                }
            case eSpwanedColorType.TopLeft:
                {
                    colorType = _selectedChromies[0];
                    break;
                }
            case eSpwanedColorType.TopRight:
                {
                    colorType = _selectedChromies[1];
                    break;
                }
            case eSpwanedColorType.BottomRight:
                {
                    colorType = _selectedChromies[2];
                    break;
                }
        }
        return colorType;
    }

    public ChromieController CreateChromie(eChromieType colorType)
    {

        ChromieController newChromieController = Lean.LeanPool.Spawn(_chromieControllerPrefab);
        if (_overrideColor != eChromieType.None)
        {
            colorType = _overrideColor;
        }
        ChromieDefenition chromieDefenition = ChromezData.Instance.GetChromie(colorType);
        newChromieController.SetChromie(chromieDefenition);

        return newChromieController;

    }

    private float TimeFactorMultiplier(float time, float multiplier, int minLevel)
    {
        if (multiplier == 1.0f)
        {
            return 1.0f;
        }

        float factor = Mathf.Pow((multiplier * 0.99f), (_currentLevel - minLevel));
        float factoredTime = time * factor;
        if (factoredTime < 0.1f)
        {
            return 0.1f;
        }
        return factoredTime;
    }

    private List<SequanceDataObject> GetAvalableSequances()
    {
        List<SequanceDataObject> avalableSequances = new List<SequanceDataObject>();

        foreach (SequanceDataObject sequance in _loadedSequances)
        {
            bool avalable = false;

            if (sequance.MinLevel == 0 && sequance.MaxLevel == 0)
            {
                avalable = true;
            }

            if (sequance.MinLevel <= _currentLevel && sequance.MaxLevel >= _currentLevel)
            {
                avalable = true;
            }

            if (sequance.GameMode != _currentGameMode && sequance.GameMode != eGameplayMode.Default)
            {
                avalable = false;
            }

            if (_usedSequances.Contains(sequance))
            {
                avalable = false;
            }

            if (avalable)
            {
                avalableSequances.Add(sequance);
            }
        }

        if (avalableSequances.Count == 0)
        {
            _usedSequances.Clear();
            return GetAvalableSequances();
        }

        return avalableSequances;
    }

    #endregion

}

/*

private enum eSpwanerPhase
{
    None,
    WaveStarted,
    SequanceStarted,
    SpwanItems,
    SequenceFinished,
    WaveFinished,
}

private eChromieType[] _selectedChromies;
private bool _paused;
private eChromieType _overrideColor;
private eSpwanerPhase _phase;
private List<WaveDefenition> _wavesList;
private List<WaveDefenition> _usedWavesList = new List<WaveDefenition>();
private List<ChromieController> _allChromiez = new List<ChromieController>();

private WaveDefenition _currentWave;
private SequanceDefenition _currentSequance;
private float _waveTimeCount;
private float _sequanceTimeCount;
private int _currentSequanceIndex;


private int _currentLevel;
private eGameplayMode _currentGameMode;

private float _currentTimeCountTarget;

// private GameData _gameData;

#endregion


#region Initialization

void Start()
{
    GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
    GameplayEventsDispatcher.Instance.OnChromieDropped += ChromieDroppedHandler;
    GameplayEventsDispatcher.Instance.OnLevelUpdate += OnLevelUpdateHandler;
    GameplayEventsDispatcher.Instance.OnGameOver += OnGameOverHandelr;
    GameplayEventsDispatcher.Instance.OnKeepPlaying += OnKeepPlayingHandler;
    _paused = true;
}

#endregion


#region Update

void Update()
{
    if (!Paused)
    {
        switch (_phase)
        {
            case eSpwanerPhase.None:
                {
                    StartNewWave();
                    break;
                }
            case eSpwanerPhase.WaveStarted:
                {
                    _waveTimeCount += Time.deltaTime;
                    _currentTimeCountTarget = TimeFactorMultiplier(_currentWave.StartDelay, _currentWave.LevelModier, _currentWave.MinLevel);
                    if (_waveTimeCount > _currentTimeCountTarget)
                    {
                        StartNextSequance();
                    }
                    break;
                }
            case eSpwanerPhase.SequanceStarted:
                {
                    _sequanceTimeCount += Time.deltaTime;
                    _currentTimeCountTarget = TimeFactorMultiplier(_currentSequance.StartInterval, _currentSequance.LevelModifier, _currentSequance.MinLevel);
                    if (_sequanceTimeCount > _currentTimeCountTarget)
                    {
                        _phase = eSpwanerPhase.SpwanItems;
                    }
                    break;
                }
            case eSpwanerPhase.SpwanItems:
                {
                    SpwanItems();
                    break;
                }
            case eSpwanerPhase.SequenceFinished:
                {
                    _sequanceTimeCount += Time.deltaTime;
                    _currentTimeCountTarget = TimeFactorMultiplier(_currentSequance.EndInterval, _currentSequance.LevelModifier, _currentSequance.MinLevel);
                    if (_sequanceTimeCount > _currentTimeCountTarget)
                    {
                        EndSequance();
                    }
                    break;
                }
            case eSpwanerPhase.WaveFinished:
                {
                    _waveTimeCount += Time.deltaTime;
                    _currentTimeCountTarget = TimeFactorMultiplier(_currentWave.EndDelay, _currentWave.LevelModier, _currentWave.MinLevel);
                    if (_waveTimeCount > _currentTimeCountTarget)
                    {
                        _phase = eSpwanerPhase.None;
                    }
                    break;
                }
            default:
                {
                    _phase = eSpwanerPhase.None;
                    break;
                }
        }
    }
}

#endregion


#region Public 

public void Init(eGameplayMode gameMode, List<ChromieDefenition> selectedColors)
{
    _spwanBasePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, 0, 10));

    _selectedChromies = new eChromieType[selectedColors.Count];
    for (int i=0; i< selectedColors.Count; i++)
    {
        _selectedChromies[i] = selectedColors[i].ChromieColor;
    }

    _wavesList = WavesDataLoader.WavesList();

    _currentGameMode = gameMode;
    _currentLevel = 0;
}

public void StartSpwaning()
{
    _paused = false;
}

public void StopSpwaning()
{
    _paused = true;
}

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

public void SetSpwanColorOverride(eChromieType overrideColor)
{
    _overrideColor = overrideColor;
}

public ChromieController CreateChromie(eChromieType colorType)
{

    ChromieController newChromieController = Lean.LeanPool.Spawn(_chromieControllerPrefab);
    if (_overrideColor != eChromieType.None)
    {
        colorType = _overrideColor;
    }
    ChromieDefenition chromieDefenition = ChromezData.Instance.GetChromie(colorType);
    newChromieController.SetChromie(chromieDefenition);

    return newChromieController;

}



#endregion


#region Events

public void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
{
    GameplayEventsDispatcher.SendChromieDeSpwaned(chromieController);
    Lean.LeanPool.Despawn(chromieController.gameObject);
}

private void ChromieDroppedHandler(ChromieController chromieController)
{
    GameplayEventsDispatcher.SendChromieDeSpwaned(chromieController);
    Lean.LeanPool.Despawn(chromieController.gameObject);
}

private void OnLevelUpdateHandler(int newLevel)
{
    _currentLevel = newLevel;
}

private void OnGameOverHandelr()
{
    Paused = true;
}

private void OnKeepPlayingHandler()
{
    _currentWave = null;
    _currentSequance = null;
    _phase = eSpwanerPhase.None;

    Paused = false;
}

#endregion


#region Private

private eChromieType GetRandomChromieForSpwan()
{
    eChromieType randomChromie = _selectedChromies[Random.Range(0, _selectedChromies.Length - 1)];
    return randomChromie;
}

private void StartNewWave()
{
    _currentWave = GetNewWave();
    _waveTimeCount = 0;
    _currentSequanceIndex = 0;
    _phase = eSpwanerPhase.WaveStarted;
}

private void StartNextSequance()
{
    if (_currentWave.SequanceList.Count > _currentSequanceIndex)
    {
        _currentSequance = _currentWave.SequanceList[_currentSequanceIndex];
        _currentSequanceIndex++;
        _sequanceTimeCount = 0;
        _phase = eSpwanerPhase.SequanceStarted;
    }
    else
    {
        _phase = eSpwanerPhase.WaveFinished;
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
    _phase = eSpwanerPhase.SequenceFinished;
}

private WaveDefenition GetNewWave()
{
    List<WaveDefenition> avalableWaves = GetAvalableWaves();

    WaveDefenition wave = avalableWaves[Random.Range(0, avalableWaves.Count)];
    return wave;
}

private List<WaveDefenition> GetAvalableWaves()
{
    List<WaveDefenition> avalableWaves = new List<WaveDefenition>();

    foreach (WaveDefenition wave in _wavesList)
    {
        bool avalable = false;

        if (wave.MinLevel == 0 && wave.MaxLevel == 0)
        {
            avalable = true;
        }

        if (wave.MinLevel <= _currentLevel && wave.MaxLevel >= _currentLevel)
        {
            avalable = true;
        }

        if (wave.GameMode != _currentGameMode && wave.GameMode != eGameplayMode.Default)
        {
            avalable = false;
        }

        if (_usedWavesList.Contains(wave))
        {
            avalable = false;
        }

        if (avalable)
        {
            avalableWaves.Add(wave);
        }
    }

    if (avalableWaves.Count == 0)
    {
        _usedWavesList.Clear();
        return GetAvalableWaves();
    }

    return avalableWaves;
}

private void SpwanItem(SpwanedItemDefenition spwanedItem)
{
    eChromieType colorType;
    if (_overrideColor != eChromieType.None)
    {
        colorType = _overrideColor;
    }
    else
    {
        colorType = ColorForSpwanColorType(spwanedItem.SpwanedColor);
    }
    ChromieController chromieController = CreateChromie(colorType);

    if (!_allChromiez.Contains(chromieController))
    {
        _allChromiez.Add(chromieController);
    }

    Vector3 spwanPosition = _spwanBasePosition;
    spwanPosition.x += spwanedItem.XPosition * SPWAN_POSITION_MULTIPLIER;
    chromieController.gameObject.transform.position = spwanPosition;

    Vector2 spwanForce = spwanedItem.ForceVector;
    spwanForce.y += FORCE_VECTOR_MODIFIER * (GameplaySettings.Instance.GameSpeedMultiplier * 0.7f);
    chromieController.gameObject.GetComponent<Rigidbody2D>().AddForce(spwanForce);
    chromieController.gameObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-90, 90);

    GameplayEventsDispatcher.SendChromieSpwaned(chromieController);
}

private eChromieType ColorForSpwanColorType(eSpwanedColorType spwanColorType)
{
    eChromieType colorType = eChromieType.None;
    switch (spwanColorType)
    {
        case eSpwanedColorType.RandomCorner:
            {
                colorType = _selectedChromies[Random.Range(0, _selectedChromies.Length)];
                break;
            }
        case eSpwanedColorType.BottomLeft:
            {
                colorType = _selectedChromies[3];
                break;
            }
        case eSpwanedColorType.TopLeft:
            {
                colorType = _selectedChromies[0];
                break;
            }
        case eSpwanedColorType.TopRight:
            {
                colorType = _selectedChromies[1];
                break;
            }
        case eSpwanedColorType.BottomRight:
            {
                colorType = _selectedChromies[2];
                break;
            }
    }
    return colorType;
}

private float TimeFactorMultiplier(float time, float multiplier, int minLevel)
{
    if (multiplier == 1.0f)
    {
        return 1.0f;
    }

    float factor = Mathf.Pow((multiplier * 0.99f), (_currentLevel - minLevel));
    float factoredTime = time * factor;
    if (factoredTime < 0.1f)
    {
        return 0.1f;
    }
    return factoredTime;
}

#endregion
}

*/
