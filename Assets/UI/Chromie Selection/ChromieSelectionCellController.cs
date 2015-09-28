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
	

	#endregion


	#region Private



	#endregion

}
