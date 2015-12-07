using UnityEngine;
using System.Collections;



public class HeartsManager : MonoBehaviour, IHearts {

	#region Public Properties

	public event HeartAddedDelegate OnHeartAdded;

	public event HeartRemovedDelegate OnHeartRemoved;

	public event OutOfHeartsDelegate OnOutOfHearts;

	#endregion

	#region Private Properties

	[SerializeField]
	private bool _heartsEnabled;

	[SerializeField]
	private int _maxHearts;

	[SerializeField]
	private float _minutesToFillHeart;

	private int _currentHeartsCount;

	private System.DateTime _lastHeartTime;

	private const string HEARTS_TIME_COUNT_KEY = "lastHeartTimeCount";

	private const string HEARTS_CURRENT_COUNT = "heartsCurrentCount";

	[SerializeField]
	private bool _enableDebug;

	#endregion


	#region Lifecycle

	// Use this for initialization
	void Start () {

		if (PlayerPrefsUtil.HasKey(HEARTS_TIME_COUNT_KEY))
		{
			string timeCountString = PlayerPrefsUtil.GetString(HEARTS_TIME_COUNT_KEY);
			if (!string.IsNullOrEmpty(timeCountString))
			{
				long savedTime = System.Convert.ToInt64(timeCountString);
				_lastHeartTime = System.DateTime.FromBinary(savedTime);
			}
		}
		else
		{
			_lastHeartTime = System.DateTime.Now;
		}

		if (PlayerPrefsUtil.HasKey(HEARTS_CURRENT_COUNT))
		{
			_currentHeartsCount = PlayerPrefsUtil.GetInt(HEARTS_CURRENT_COUNT);
		}
		else
		{
			_currentHeartsCount = _maxHearts;
			PlayerPrefsUtil.SetInt(HEARTS_CURRENT_COUNT, _currentHeartsCount);
		}
	}

	void OnApplicationQuit() {
		UpdateHeartsCount();
		PlayerPrefsUtil.Save();
	}
	
	// Update is called once per frame
	void Update () {

		if (_heartsEnabled)
		{
			if (_currentHeartsCount < _maxHearts)
			{
				System.TimeSpan timeSinceLastHeart = System.DateTime.Now - _lastHeartTime;
				if (timeSinceLastHeart.TotalMinutes > _minutesToFillHeart)
				{
					_lastHeartTime = _lastHeartTime.AddMinutes(_minutesToFillHeart);
					RefillHearts();
				}
			}
		}
	}

	#endregion


	#region IHearts Implementation

	public bool HeartsEnabled
	{
		set
		{
			_heartsEnabled = value;
		}
		get
		{
			return _heartsEnabled;
		}
	}

	public int HeartsCount
	{
		get
		{
			return _currentHeartsCount;
		}
	}

	public int MaxHeartsCount
	{
		get
		{
			return _maxHearts;
		}
	}

	public System.TimeSpan TimeToNextHeart
	{
		get
		{
			if (_currentHeartsCount >= _maxHearts)
			{
				return System.TimeSpan.Zero;
			}
			return _lastHeartTime.AddMinutes(_minutesToFillHeart) - System.DateTime.Now;
		}
	}

	public void ReduceHeart()
	{
		if (_currentHeartsCount <= 0)
		{
			Debug.LogError("ERROR - trying to reduce hearts when current hearts count is 0");
			return;
		}
		if (_currentHeartsCount == _maxHearts)
		{
			ResetheartTimer();
		}
		_currentHeartsCount--;
		if (TimeToNextHeart == System.TimeSpan.Zero)
		{
			ResetheartTimer();
		}
		if (OnHeartRemoved != null)
		{
			OnHeartRemoved();
		}
		if (_currentHeartsCount == 0)
		{
			if (OnOutOfHearts != null)
			{
				OnOutOfHearts();
			}
		}
		UpdateHeartsCount();
	}

	public void RefillHearts(int amount = 1)
	{
		_currentHeartsCount += amount;
		if (_currentHeartsCount > _maxHearts)
		{
			_currentHeartsCount = _maxHearts;
			ResetheartTimer();
		}
		else
		{
			if (OnHeartAdded != null)
			{
				OnHeartAdded();
			}
		}
		UpdateHeartsCount();
	}

	#endregion


	#region Private

	private void UpdateHeartsCount()
	{
		PlayerPrefsUtil.SetInt(HEARTS_CURRENT_COUNT, _currentHeartsCount);
		PlayerPrefsUtil.Save();
	}


	private void ResetheartTimer()
	{
		_lastHeartTime = System.DateTime.Now;
		PlayerPrefsUtil.SetString(HEARTS_TIME_COUNT_KEY, _lastHeartTime.ToBinary().ToString());
	}

	#endregion


	#region DEBUG

	void OnGUI()
	{
		if (_enableDebug)
		{
			GUILayout.BeginVertical();
			GUILayout.Label("Hearts: " + _currentHeartsCount + "/" + _maxHearts);
			System.TimeSpan timeToNextHeart = TimeToNextHeart;
			GUILayout.Label("Next Heart: " + timeToNextHeart.Minutes + ":" + timeToNextHeart.Seconds);
			if (GUILayout.Button("Reduce Heart"))
			{
				ReduceHeart();
			}
			if (GUILayout.Button("Refill 1 heart"))
			{
				RefillHearts(1);
			}
			GUILayout.EndHorizontal();
		}
	}

	#endregion
}
