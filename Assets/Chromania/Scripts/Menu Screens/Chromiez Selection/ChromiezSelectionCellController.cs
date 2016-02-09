using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChromiezSelectionCellController : MonoBehaviour {

    #region Public Properties

    public delegate void CellSelectedDelegate(ChromiezSelectionCellController chromieCell);

    public event CellSelectedDelegate OnCellSelected;

    #endregion

    #region Private Properties

    [SerializeField]
    private Image _chromieIcon;

    [SerializeField]
    private Image _background;

    [SerializeField]
    private Image _overlay;

    private ChromieDataObject _chromieData;

    #endregion



    #region Public

    public void SetChromie(ChromieDataObject chromieData)
    {
        _chromieData = chromieData;

        if (_background != null)
        {
            Color color = ChromieData.ColorValue;
            color.a = 1;
            _background.color = color;
        }

        if (_chromieIcon != null)
        {
            IChromiezAssetsCache chromieAssetCache = ComponentFactory.GetAComponent<IChromiezAssetsCache>();
            if (chromieAssetCache != null)
            {
                _chromieIcon.sprite = chromieAssetCache.GetChromieSprite(_chromieData.ChromieColor);
            }
        }
    }

    public void CellClickAction()
    {
        if (OnCellSelected != null)
        {
            OnCellSelected(this);
        }
    }

    public void SetSelected(bool state)
    {
        if (_overlay != null)
        {
            _overlay.gameObject.SetActive(state);
        }
    }

    public ChromieDataObject ChromieData
    {
        get
        {
            return _chromieData;
        }
    }

    #endregion
}
