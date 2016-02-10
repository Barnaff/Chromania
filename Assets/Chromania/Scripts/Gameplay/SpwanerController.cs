using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpwanerController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private eChromieType[] _selectedChromies;

    [SerializeField]
    private Vector3 _spwanBasePosition;

    private const float FORCE_VECTOR_MODIFIER = 450;
    private const float SPWAN_POSITION_MULTIPLIER = 0.032f;


    private enum eSpwanerPhase
    {
        None,
        WaveStarted,
        SequanceStarted,
        SpwanItems,
        SequenceFinished,
        WaveFinished,
    }

    private bool _paused;

    private eChromieType _overrideColor;

    private eSpwanerPhase _phase;

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

    private List<WaveDefenition> _wavesList;

    private int _currentLevel;

    private WaveDefenition _currentWave;

    private SequanceDefenition _currentSequance;

    private float _waveTimeCount;

    private float _sequanceTimeCount;

    private int _currentSequanceIndex;

    #endregion


    // Use this for initialization
    void Start () {

        GameplayEventsDispatcher.Instance().OnChromieDropped += ChromieDroppedHandler;
	}
	
	// Update is called once per frame
	void Update ()
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
                        if (_waveTimeCount > _currentWave.StartDelay)
                        {
                            StartNextSequance();
                        }
                        break;
                    }
                case eSpwanerPhase.SequanceStarted:
                    {
                        _sequanceTimeCount += Time.deltaTime;
                        if (_sequanceTimeCount > _currentSequance.StartInterval)
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
                        if (_sequanceTimeCount > _currentSequance.EndInterval)
                        {
                            EndSequance();
                        }
                        break;
                    }
                case eSpwanerPhase.WaveFinished:
                    {
                        _waveTimeCount += Time.deltaTime;
                        if (_waveTimeCount > _currentWave.EndDelay)
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

    #region Public 

    public void Init(eChromieType[] selectedCollors, int level)
    {
        _spwanBasePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, 0, 10));

        _selectedChromies = selectedCollors;

        _wavesList = WavesDataLoader.WavesList();

        // InvokeRepeating("MockSpwan", 1.0f, 1.0f);
    }

    #endregion


    private void MockSpwan()
    {
        eChromieType randomChromie = GetRandomChromieForSpwan();
        Vector3 spwanPosition = Vector3.zero;
        Vector2 forceVector = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        SpwanChromie(randomChromie, spwanPosition, forceVector);
    }

    private void SpwanChromie(eChromieType chromieType, Vector3 position, Vector2 direction)
    {
        GameObject chromiePrefab = GetChromiePrefab(chromieType);
        GameObject newChromie = Lean.LeanPool.Spawn(chromiePrefab);
        ChromieController chromieController = newChromie.GetComponent<ChromieController>();
        chromieController.Init();

        Vector3 spwanPosition = _spwanBasePosition;
        spwanPosition.x += position.x * SPWAN_POSITION_MULTIPLIER;
        newChromie.transform.position = spwanPosition;
        direction.y += FORCE_VECTOR_MODIFIER;
        newChromie.GetComponent<Rigidbody2D>().AddForce(direction);
        newChromie.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-90, 90);

        GameplayEventsDispatcher.SendChromieSpwaned(chromieController);
    }

    private GameObject GetChromiePrefab(eChromieType chromieType)
    {
        IChromiezAssetsCache chromiezAssetsCache = ComponentFactory.GetAComponent<IChromiezAssetsCache>();
        if (chromiezAssetsCache != null)
        {
            return chromiezAssetsCache.GetGameplayChromie(chromieType);
        }
        return null;
    }

    private eChromieType GetRandomChromieForSpwan()
    {
        eChromieType randomChromie = _selectedChromies[Random.Range(0, _selectedChromies.Length - 1)];
        return randomChromie;
    }


    #region Events

    private void ChromieDroppedHandler(ChromieController chromieController)
    {
        Lean.LeanPool.Despawn(chromieController.gameObject);
    }

    #endregion


    #region Private

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
        WaveDefenition wave = _wavesList[Random.Range(0, _wavesList.Count)];
        return wave;
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
        GameObject chromie = CreateChromie(colorType);

        Vector3 spwanPosition = _spwanBasePosition;
        spwanPosition.x += spwanedItem.XPosition * 0.032f;
        chromie.transform.position = spwanPosition;

        Vector2 spwanForce = spwanedItem.ForceVector;
        spwanForce.y += 450;
        chromie.GetComponent<Rigidbody2D>().AddForce(spwanForce);
        chromie.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-90, 90);

        GameplayEventsDispatcher.SendChromieSpwaned(chromie.GetComponent<ChromieController>());
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

    private GameObject CreateChromie(eChromieType colorType)
    {
        GameObject chromiePrefab = GetChromiePrefab(colorType);
        GameObject newChromie = Lean.LeanPool.Spawn(chromiePrefab);
        ChromieController chromieController = newChromie.GetComponent<ChromieController>();
        chromieController.Init();

        return newChromie;

    }

    #endregion

}
