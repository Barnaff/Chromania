using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotsButtonsController : MonoBehaviour {

	public delegate void SlotsButtonDelegate();

	public event SlotsButtonDelegate OnSpin;
	public event SlotsButtonDelegate OnMaxBet;
	public event SlotsButtonDelegate OnPayTable;
	public event SlotsButtonDelegate OnAddLine;
	public event SlotsButtonDelegate OnReduceLine;
	public event SlotsButtonDelegate OnAddBet;
	public event SlotsButtonDelegate OnReduceBet;

	[SerializeField]
	private GameObject[] _panelEelements;

	[SerializeField]
	private Text _linesLabel;

	[SerializeField]
	private Text _betLabel;

	[SerializeField]
	private Text _totalLabel;
	
	[SerializeField]
	private Text _winLabel;

	[SerializeField]
	private Color _activeColor;

	[SerializeField]
	private Color _inactiveColor;

	[SerializeField]
	private GameObject _spinButton;

	[SerializeField]
	private GameObject _maxBetButton;

	private bool _isAnimatingWin = false;


	public void SetPanelItemsState(bool state)
	{
		foreach (GameObject element in _panelEelements)
		{
			if (element.GetComponent<Button>() != null)
			{
				element.GetComponent<Button>().enabled = state;

				if (state)
				{
					element.GetComponent<Image>().color = _activeColor;
				}
				else
				{
					element.GetComponent<Image>().color = _inactiveColor;
				}
			}
		}
	}

	public void SetPanelSpinEnabled(bool enabled)
	{
		if (_spinButton != null)
		{
			_spinButton.GetComponent<Button>().enabled = enabled;
			if (enabled)
			{
				_spinButton.GetComponent<Image>().color = _activeColor;
			}
			else
			{
				_spinButton.GetComponent<Image>().color = _inactiveColor;
			}
		}

		if (_maxBetButton != null)
		{
			_maxBetButton.GetComponent<Button>().enabled = enabled;
			if (enabled)
			{
				_maxBetButton.GetComponent<Image>().color = _activeColor;
			}
			else
			{
				_maxBetButton.GetComponent<Image>().color = _inactiveColor;
			}
		}
	}


	public void SpinButtonAction()
	{
		if (OnSpin != null)
		{
			OnSpin();
		}
	}


	public void MaxBetButtonAction()
	{
		if (OnMaxBet != null)
		{
			OnMaxBet();
		}
	}

	public void PayTableButtonAction()
	{
		if (OnPayTable != null)
		{
			OnPayTable();
		}
	}

	public void AddLineButtonAction()
	{
		if (OnAddLine != null)
		{
			OnAddLine();
		}
	}

	public void ReduceLineButtonAction()
	{
		if (OnReduceLine != null)
		{
			OnReduceLine();
		}
	}

	public void AddBetButtonAction()
	{
		if (OnAddBet != null)
		{
			OnAddBet();
		}
	}

	public void ReduceBetButtonAction()
	{
		if (OnReduceBet != null)
		{
			OnReduceBet();
		}
	}

	public void SetLines(int linesCount)
	{
		_linesLabel.text = linesCount.ToString();
	}

	public void SetBet(float betCount)
	{
		_betLabel.text = betCount.ToString();
	}

	public void SetTotal(float totalCount)
	{
		_totalLabel.text = totalCount.ToString();
	}

	public void SetWin(float winCount, bool animate)
	{
		_winLabel.text = winCount.ToString();

		if (animate && !_isAnimatingWin)
		{
			StartCoroutine(AnimateWinLabel());
		}
		else
		{
			_isAnimatingWin = false;
		}
	}

	IEnumerator AnimateWinLabel()
	{
		Outline outline = _winLabel.gameObject.GetComponent<Outline>();
		if (outline == null)
		{
			outline = _winLabel.gameObject.AddComponent<Outline>();
			Color color = Color.yellow;
			color.a = 0.5f;
			outline.effectColor = color;
			outline.effectDistance = new Vector2(2,-2);
		}
		_isAnimatingWin = true;
		while (_isAnimatingWin)
		{
			outline.enabled = !outline.enabled;
			yield return new WaitForSeconds(0.2f);
		}

		outline.enabled = false;
	}
}
