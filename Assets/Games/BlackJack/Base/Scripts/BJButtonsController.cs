using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BJButtonsController : MonoBehaviour
{

    public delegate void BJButtonActionDelegate();

    public event BJButtonActionDelegate OnDealButtonAction;
    public event BJButtonActionDelegate OnHitButtonAction;
    public event BJButtonActionDelegate OnStandButtonAction;
    public event BJButtonActionDelegate OnSplitButtonAction;
    public event BJButtonActionDelegate OnDoubleButtonAction;
    public event BJButtonActionDelegate OnPlusButtonAction;
    public event BJButtonActionDelegate OnMinusButtonAction;


    [SerializeField]
    private GameObject _dealButton;

    [SerializeField]
    private GameObject _hitButton;

    [SerializeField]
    private GameObject _standButton;

    [SerializeField]
    private GameObject _splitButton;

    [SerializeField]
    private GameObject _doubleButton;

    [SerializeField]
    private GameObject _plusButton;

    [SerializeField]
    private GameObject _minusButton;

    [SerializeField]
    private Text _betAmountLabel;


    #region Initialize

    public void Init()
    {
		_dealButton.SetActive(false);
		_hitButton.SetActive(false);
		_standButton.SetActive(false);
		_splitButton.SetActive(false);
		_doubleButton.SetActive(false);
    }

    #endregion


    #region Public

    public void EnableDealButton()
    {

        _dealButton.GetComponent<Button>().interactable = true;
        _dealButton.SetActive(true);
        _plusButton.GetComponent<Button>().interactable = true;
        _minusButton.GetComponent<Button>().interactable = true;

        _dealButton.SetActive(true);
        _hitButton.SetActive(false);
        _standButton.SetActive(false);
        _splitButton.SetActive(false);
        _doubleButton.SetActive(false);
    }

    public void EnableGameActionsButtons()
    {
        _dealButton.SetActive(false);
        _hitButton.SetActive(true);
        _standButton.SetActive(true);
        _splitButton.SetActive(true);
        _doubleButton.SetActive(true);

        _plusButton.GetComponent<Button>().interactable = false;
        _minusButton.GetComponent<Button>().interactable = false;
        _hitButton.GetComponent<Button>().interactable = true;
        _standButton.GetComponent<Button>().interactable = true;
        _splitButton.GetComponent<Button>().interactable = false;
        _doubleButton.GetComponent<Button>().interactable = false;
    }

    public void LockGameplayButtons()
    {
        _hitButton.GetComponent<Button>().interactable = false;
        _standButton.GetComponent<Button>().interactable = false;
        _splitButton.GetComponent<Button>().interactable = false;
        _doubleButton.GetComponent<Button>().interactable = false;
    }

    public void EnableSplitButton()
    {
		_splitButton.GetComponent<Button>().interactable = true;
    }

	public void DisableSplitButton()
	{
		_splitButton.GetComponent<Button>().interactable = false;
	}

    public void EnableDoubleButton()
    {
        _doubleButton.GetComponent<Button>().interactable = true;
    }

	public void DisableDoubleButton()
	{
		_doubleButton.GetComponent<Button>().interactable = false;
	}

	public void SetBetAmount(int betAmount)
	{
		_betAmountLabel.text = betAmount.ToString();
	}

    #endregion


    #region User Ineractions

    public void DealButtonAction()
    {
        if (OnDealButtonAction != null)
        {
            OnDealButtonAction();
        }
    }

    public void HitButtonAction()
    {
        if (OnHitButtonAction != null)
        {
            OnHitButtonAction();
        }
    }

    public void StandButtonAction()
    {
        if (OnStandButtonAction != null)
        {
            OnStandButtonAction();
        }
    }

    public void SplitButtonAction()
    {
        if (OnSplitButtonAction != null)
        {
            OnSplitButtonAction();
        }
    }

    public void DoubleButtonAction()
    {
        if (OnDoubleButtonAction != null)
        {
            OnDoubleButtonAction();
        }
    }

    public void PlusButtonAction()
    {
        if (OnPlusButtonAction != null)
        {
            OnPlusButtonAction();
        }
    }

    public void MinusButtonAction()
    {
        if (OnMinusButtonAction != null)
        {
            OnMinusButtonAction();
        }
    }

    #endregion

}

/*

public void Init()
{
    _splitButton.GetComponent<Button>().interactable = true;
    _doubleButton.GetComponent<Button>().interactable = true;
}

public void DispalyDeal()
{
    _dealButton.SetActive(true);
    _hitButton.SetActive(false);
    _standButton.SetActive(false);
    _splitButton.SetActive(false);
    _doubleButton.SetActive(false);

    SetDealButtonState(true);
    SetBetButtonsState(true);
}

public void RoundStarted()
{
    _dealButton.SetActive(false);
    _hitButton.SetActive(true);
    _standButton.SetActive(true);
    _splitButton.SetActive(true);
    _doubleButton.SetActive(true);

    SetAllButtonsState(true);
    SetBetButtonsState(false);
}

public void HideAllButtons()
{
    _dealButton.SetActive(false);
    _hitButton.SetActive(false);
    _standButton.SetActive(false);
    _splitButton.SetActive(false);
    _doubleButton.SetActive(false);
}

public void SetBetAmount(int betAmount)
{
    _betAmountLabel.text = betAmount.ToString();
}

public void SetDealButtonState(bool state)
{
    if (state)
    {
        _dealButton.GetComponent<Image>().color = Color.white;
    }
    else
    {
        _dealButton.GetComponent<Image>().color = Color.gray;
    }
    _dealButton.GetComponent<Button>().enabled = state;
}

public void SetDoubleState(bool state)
{
    Debug.Log("set double button state: " + state);
    if (state)
    {
        _doubleButton.GetComponent<Image>().color = Color.white;
    }
    else
    {
        _doubleButton.GetComponent<Image>().color = Color.gray;
    }
    _doubleButton.GetComponent<Button>().enabled = state;
}

public void SetSplitState(bool state)
{
    if (state)
    {
        _splitButton.GetComponent<Image>().color = Color.white;
    }
    else
    {
        _splitButton.GetComponent<Image>().color = Color.gray;
    }
    _splitButton.GetComponent<Button>().enabled = state;
}

private void SetBetButtonsState(bool state)
{
    if (state)
    {
        _plusButton.GetComponent<Image>().color = Color.white;
        _minusButton.GetComponent<Image>().color = Color.white;
    }
    else
    {
        _plusButton.GetComponent<Image>().color = Color.gray;
        _minusButton.GetComponent<Image>().color = Color.gray;
    }
    _plusButton.GetComponent<Button>().enabled = state;
    _minusButton.GetComponent<Button>().enabled = state;
}

public void SetAllButtonsState(bool state)
{
    Color color = Color.white;
    if (!state)
    {
        color = Color.gray;
    }

    _hitButton.GetComponent<Image>().color = color;
    _standButton.GetComponent<Image>().color = color;
    _splitButton.GetComponent<Image>().color = color;
    _doubleButton.GetComponent<Image>().color = color;

    _hitButton.GetComponent<Button>().enabled = state;
    _standButton.GetComponent<Button>().enabled = state;
    _splitButton.GetComponent<Button>().enabled = state;
    _doubleButton.GetComponent<Button>().enabled = state;
}

#region User Ineractions

public void DealButtonAction()
{
    if (OnDealButtonAction != null)
    {
        OnDealButtonAction();
    }
}

public void HitButtonAction()
{
    if (OnHitButtonAction != null)
    {
        OnHitButtonAction();
    }
}

public void StandButtonAction()
{
    if (OnStandButtonAction != null)
    {
        OnStandButtonAction();
    }
}

public void SplitButtonAction()
{
    if (OnSplitButtonAction != null)
    {
        OnSplitButtonAction();
    }
}

public void DoubleButtonAction()
{
    if (OnDoubleButtonAction != null)
    {
        OnDoubleButtonAction();
    }
}

public void PlusButtonAction()
{
    if (OnPlusButtonAction != null)
    {
        OnPlusButtonAction();	
    }
}

public void MinusButtonAction()
{
    if (OnMinusButtonAction != null)
    {
        OnMinusButtonAction();
    }
}

#endregion
}

*/
