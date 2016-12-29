using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ChromieSelectionItemCellController : MonoBehaviour {

    #region Public Properties

    public delegate void ItemCellSelectedDelegate(ChromieSelectionItemCellController cellController);

    public ItemCellSelectedDelegate OnItemCellSelected;

    #endregion


    #region Private Properties

    [SerializeField]
    private Image _chromieImage;

    [SerializeField]
    private GameObject _lockOverlay;

    [SerializeField]
    private GameObject _selectedIndicator;

    [SerializeField]
    private bool _isSelected;

    [SerializeField]
    private bool _isLocked;

    [SerializeField]
    private ChromieDefenition _chromieDefenition;

    #endregion


    #region Publc

    public void SetCell(ChromieDefenition chromieDefenition, bool isSelected)
    {
        _chromieDefenition = chromieDefenition;

        if (_chromieDefenition.ChromieSprite != null)
        {
            _chromieImage.sprite = _chromieDefenition.ChromieSprite;
        }

        if (InventoryManager.Instance.HasItem(chromieDefenition.ChromieColor.ToString()))
        {
            _lockOverlay.SetActive(false);
        }
        else
        {
            _lockOverlay.SetActive(true);
        }

        _selectedIndicator.SetActive(isSelected);
    }

    public ChromieDefenition ChromieDefenition
    {
        get
        {
            return _chromieDefenition;
        }
    }

    public bool Selected
    {
        set
        {
            _isSelected = value;
            _selectedIndicator.SetActive(_isSelected);
            DisplaySelectedAnimation(_isSelected);
        }
        get
        {
            return _isSelected;
        }
    }

    #endregion


    #region User Interactions

    public void CellClickAction()
    {
        if (OnItemCellSelected != null)
        {
            OnItemCellSelected(this);
        }
    }

    #endregion

    #region Private 

    private void DisplaySelectedAnimation(bool selected)
    {
        if (selected)
        {
            _chromieImage.transform.DOScale(0, 0.3f).SetEase(Ease.OutCirc);
        } 
        else
        {
            _chromieImage.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic);
        }
    }

    #endregion
}
