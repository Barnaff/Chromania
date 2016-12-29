using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField]
    private Image _whirlwinImage;

    [SerializeField]
    private Image _backgroundImage;

    [SerializeField]
    private Color _emptyColor;

    private Vector3 _targetCharacterPosition = Vector3.zero;

    #endregion


    #region Public

    public void SetSelection(ChromieDefenition chromieDefenition)
    {
        _targetCharacterPosition = this.transform.position;
        if (chromieDefenition != null && chromieDefenition.ChromieSprite != null)
        {
            _selectedItemImage.gameObject.SetActive(true);
            _selectedItemImage.sprite = chromieDefenition.ChromieSprite;

            _whirlwinImage.color = chromieDefenition.ColorValue + new Color(0.1f, 0.1f, 0.1f, 1) ;
            _backgroundImage.color = chromieDefenition.ColorValue;

            if (_chromieDefenition != chromieDefenition)
            {
                _selectedItemImage.gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
            }

        }
        else
        {
            _selectedItemImage.gameObject.SetActive(false);
            _whirlwinImage.color = _emptyColor + new Color(0.1f, 0.1f, 0.1f, 1);
            _backgroundImage.color = _emptyColor;
        }
        _chromieDefenition = chromieDefenition;
    }

    public ChromieDefenition ChromieDefenition
    {
        get
        {
            return _chromieDefenition;
        }
    }

    #endregion

    void Update()
    {
        if (Random.Range(0,100) < 20)
        {
            _targetCharacterPosition = this.transform.position + (Random.insideUnitSphere * 10f);
        }
        else
        {
            _selectedItemImage.transform.position = Vector3.Lerp(_selectedItemImage.transform.position, _targetCharacterPosition, Time.deltaTime);
        }
    }


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
