using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public delegate void OnGameOverDelegate();

public class LivesPanelController : MonoBehaviour {

	#region Public Properties

	public GameObject LivesIndicatorPrefab;

	public Sprite FuulLiveSprite;

	public Sprite EmptyLiveSprite;

	public int NumberOfLivesSlots;

	public int NumberOfLives;

	public int CurrentLives;

	public GameObject HitIndicatorPrefab;

	public List<GameObject> LivesIndicators;

	public OnGameOverDelegate OnGameOver;

	#endregion
	

	#region Initialize

	void Awake () {
	
		this.gameObject.SetActive(false);
	}

	#endregion


	#region Public

	public void EnableLives()
	{
		this.gameObject.SetActive(true);

		CurrentLives = NumberOfLives;
		for (int i=0 ; i< LivesIndicators.Count ; i++)
		{
			GameObject liveIndicator = LivesIndicators[i];
			if (i >= NumberOfLivesSlots)
			{
				liveIndicator.SetActive(false);
			}
		}
		UpdateLives();
	}

	public void ReduceLife()
	{
		CurrentLives--;
		UpdateLives();

		if (CurrentLives < 0)
		{
			if (OnGameOver != null)
			{
				OnGameOver();
			}
		}
	}

	public void AddExtraLife()
	{
        if (CurrentLives < NumberOfLivesSlots)
        {
            CurrentLives++;
            UpdateLives();
        }
	}

	public void AddExtraLifeSlot()
	{
		NumberOfLivesSlots++;
		UpdateLives();
	}

	public void DisplayHit(Vector3 chromiePosition)
	{
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(chromiePosition);
		Vector3 indicatorPosition = screenPosition;

		if (screenPosition.x < 0)
		{
			indicatorPosition.x = 50;
		}

		if (screenPosition.x > Screen.width)
		{
			indicatorPosition.x = Screen.width - 50;
		}

		if (screenPosition.y < 0)
		{
			indicatorPosition.y = 50;
		}

		if (screenPosition.y > Screen.height)
		{
			indicatorPosition.y = Screen.height - 50;
		}
		indicatorPosition = Camera.main.ScreenToWorldPoint(indicatorPosition);

		GameObject hitIndicator = Instantiate(HitIndicatorPrefab, indicatorPosition, Quaternion.identity) as GameObject;

		iTween.PunchScale(hitIndicator, iTween.Hash("time", 0.5f, "x", 1.2f, "y", 1.2f));
		iTween.ScaleTo(hitIndicator, iTween.Hash("time", 0.5f, "x", 0, "y", 0, "delay", 1.0f));
	}

	#endregion


	#region Private

	private void UpdateLives(bool animated = true)
	{
		for (int i=0 ; i< LivesIndicators.Count ; i++)
		{
			GameObject liveIndicator = LivesIndicators[i];
			
			if (i < CurrentLives)
			{
				liveIndicator.GetComponent<Image>().sprite = FuulLiveSprite;
			}
			else
			{
				liveIndicator.GetComponent<Image>().sprite = EmptyLiveSprite;
			}
		}
	}

	#endregion
}
