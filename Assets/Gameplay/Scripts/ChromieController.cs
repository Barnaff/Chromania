using UnityEngine;
using System.Collections;

public delegate void ChromieCollectedDelegate(ChromieController chromieController);

public delegate void ChromieDroppedDelegate(ChromieController chromieController);

public delegate void OnChromieRemovedDelegate(ChromieController chromieController);

public class ChromieController : MonoBehaviour {

	#region Public Properties

	/// <summary>
	/// The type.
	/// </summary>
	public ColorType ChromieType;

	/// <summary>
	/// The is grabbed.
	/// </summary>
	public bool IsGrabbed;

	public bool IsActive;

	public ChromieCollectedDelegate OnChromieCollected;

	public ChromieDroppedDelegate OnChromieDropped; 

	public OnChromieRemovedDelegate OnChromieRemoved;

	public ChromieDataItem ChromieData;

	#endregion


	#region Private Properties

	private Bounds _screenBounds;

	#endregion


	#region Initialize

	// Use this for initialization
	void Start () {
		_screenBounds = Camera.main.gameObject.GetComponent<CameraController>().OrthographicBounds();
	}

	void OnEnable()
	{
		IsActive = false;
	}

	#endregion


	#region Update
	
	// Update is called once per frame
	void LateUpdate () {
	
		if (this.transform.position.x < -_screenBounds.size.x || this.transform.position.x > _screenBounds.size.x ||
		    this.transform.position.y < -_screenBounds.size.y || this.transform.position.y > _screenBounds.size.y)
		{
			if (OnChromieDropped != null)
			{
				OnChromieDropped(this);
			}
		}
	}

	#endregion


	#region Public

	public void GrabChromie()
	{
		IsGrabbed = true;
		IsActive = true;
	}

	public void ReleaseChromie()
	{
		IsGrabbed = false;
	}

	public void SetChromie(ColorType colorType)
	{
		ChromieType = colorType;
	}

	public void RemoveChromie()
	{
		if (OnChromieRemoved != null)
		{
			OnChromieRemoved(this);
		}
	}

	#endregion


	#region Physics

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (IsActive)
		{
			ColorZoneController colorZone = collider.gameObject.GetComponent<ColorZoneController>() as ColorZoneController;
			if (colorZone != null)
			{
				if (colorZone.ColorZoneType == ChromieType)
				{
					if (OnChromieCollected != null)
					{
						OnChromieCollected(this);
					}
					
					colorZone.CollectChromie(this);
				}
			}
		}
	}


	#endregion
}
