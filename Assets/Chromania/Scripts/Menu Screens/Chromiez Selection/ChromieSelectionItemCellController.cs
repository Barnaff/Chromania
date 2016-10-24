using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChromieSelectionItemCellController : MonoBehaviour {

    #region Public Properties

    public delegate void ItemCellSelectedDelegate(ChromieSelectionItemCellController cellController);

    public ItemCellSelectedDelegate OnItemCellSelected;

    #endregion


    #region Private Properties

    [SerializeField]
    private Image _chromieImage;

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
}
