using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void SelectChromieCell(ChromieSelectionCellController chromieCell);

public class ChromieSelectionCellController : MonoBehaviour {

    public SelectChromieCell OnSelectChromieCell;

    #region Public Properties

    /// <summary>
    /// The chromie icon.
    /// </summary>
    public Image ChromieIcon;

	/// <summary>
	/// The background.
	/// </summary>
	public Image Background;

	/// <summary>
	/// The overlay.
	/// </summary>
	public Image Overlay;

	/// <summary>
	/// The lock icon.
	/// </summary>
	public Image LockIcon;

	/// <summary>
	/// The type of the cell color.
	/// </summary>
	public ColorType CellColorType;

	#endregion


	#region private Properties

    [SerializeField]
	private ChromieDataItem _chromieDataItem;

	#endregion

	#region Initialize

	// Use this for initialization
	void Start () {

        SetSelectState(false);
    }

	#endregion
	

	#region Public

	public void SetChromie(ChromieDataItem chromieDataItem)
	{
		_chromieDataItem = chromieDataItem;

        CellColorType = chromieDataItem.ChromieColor;

        Sprite chromieSprite = ChromiezSpritesFactory.GetChromieSprite(CellColorType);
        if (chromieSprite != null)
        {
            ChromieIcon.sprite = chromieSprite;
        }

        Color backgroundColor = chromieDataItem.ColorValue;
        backgroundColor.a = 0.5f;

        this.gameObject.GetComponent<Image>().color = backgroundColor;

        this.gameObject.name = CellColorType.ToString() + " Cell";
    }

    public void SelectChromieCellButtonAction()
    {
        if (OnSelectChromieCell != null)
        {
            OnSelectChromieCell(this);
        }
    }

    public void SetSelectState(bool isSelected, bool animated = false)
    {
        float animationDuration = 0.0f;
        if (animated)
        {
            animationDuration = 0.3f;
        }

        if (isSelected)
        {   
            Overlay.CrossFadeAlpha(1.0f, animationDuration, true);
        }
        else
        {
            Overlay.CrossFadeAlpha(0.0f, animationDuration, true);
        }

		ChromieIcon.gameObject.SetActive(!isSelected);

        if (isSelected && animated)
        {
            iTween.ScaleFrom(ChromieIcon.gameObject, iTween.Hash("time", 0.3f, "x", 0.1f, "y", 0.1f, "easetype", iTween.EaseType.easeOutElastic));

        }
    }
	

	#endregion


	#region Private



	#endregion

}
