using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChromieSelectionSelectedCellController : MonoBehaviour {

    #region Public Properties

    public delegate void SelectionCellSelectedDelegate(ChromieSelectionSelectedCellController cellController);

    public SelectionCellSelectedDelegate OnSelectionCellSelected;

    #endregion


    #region Private Properties

    [SerializeField]
    private Image _selectedItemImage;

    [SerializeField]
    private ChromieDefenition _chromieDefenition;

    #endregion


    #region Public

    public void SetSelection(ChromieDefenition chromieDefenition)
    {
        _chromieDefenition = chromieDefenition;
        if (_chromieDefenition != null && _chromieDefenition.ChromieSprite != null)
        {
            _selectedItemImage.gameObject.SetActive(true);
            _selectedItemImage.sprite = _chromieDefenition.ChromieSprite;
        }
        else
        {
            _selectedItemImage.gameObject.SetActive(false);
        }
    }

    public ChromieDefenition ChromieDefenition
    {
        get
        {
            return _chromieDefenition;
        }
    }

    #endregion


    #region User Interactions

    public void CellClickAction()
    {
        if (OnSelectionCellSelected != null)
        {
            OnSelectionCellSelected(this);
        }
    }

    #endregion
}
