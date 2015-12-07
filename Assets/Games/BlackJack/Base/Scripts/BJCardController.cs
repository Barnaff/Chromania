using UnityEngine;
using System.Collections;

public class BJCardController : MonoBehaviour {

    /*
	public enum eCardType
	{
		Diamonds,
		Clover,
		Heart,
		Spade,
	}

	[SerializeField]
	private int _cardId;

	[SerializeField]
	private int _cardValue;

	[SerializeField]
	private eCardType _cardType;

	

	private bool _isFlipped;

	private Quaternion _originalRotation;
	*/

    [SerializeField]
    private BJCardModel _cardModel;

    [SerializeField]
    private SpriteRenderer _cardSprite;

    [SerializeField]
    private GameObject _cardBack;

	[SerializeField]
	private bool _isActive;

   // private Quaternion _originalRotation;

    // Use this for initialization
    void Start () {
	
		//_originalRotation = this.transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {
	


	}

	public bool IsFlipped
	{
		get
		{
			return _cardModel.IsFlipped;
		}
	}

    public void SetCardModel(BJCardModel cardModel, Sprite cardSprite)
    {
		_isActive = true;
        _cardModel = cardModel;
        _cardSprite.sprite = cardSprite;

        _cardSprite.gameObject.SetActive(false);
        _cardBack.SetActive(true);
		iTween.ColorTo(_cardSprite.gameObject, iTween.Hash("time", 0.0f, "color", Color.white));

		this.transform.rotation = Quaternion.Euler(Vector3.zero);

		Vector3 offset = Random.insideUnitSphere * 0.1f;
		offset.z = 0;
		_cardSprite.transform.localPosition = offset;
		_cardBack.transform.localPosition = offset;

		Quaternion rotationOffset = Quaternion.Euler(new Vector3(0,0,Random.Range(-10,10)));
		_cardSprite.transform.rotation = rotationOffset;
		rotationOffset.z = rotationOffset.z * -1;
		_cardBack.transform.rotation = rotationOffset;
    }

	public void SetCardIndex(int index)
	{
		_cardSprite.sortingOrder = index;
		_cardBack.GetComponent<SpriteRenderer>().sortingOrder = index;

		SetCardOrder();
	}

	public void SetCardOrder()
	{
		int order = _cardSprite.sortingOrder;
		if (_isActive)
		{
			order += 10;
		}
		else
		{
			order -= 10;
		}
		_cardSprite.sortingOrder = order;
		_cardBack.GetComponent<SpriteRenderer>().sortingOrder = order;
	}

    public IEnumerator Flip(bool animated = true)
    {
        if (_cardModel.IsFlipped)
        {
            yield break;
        }
        _cardModel.IsFlipped = true;
        if (animated)
        {
            float inTime = 0.5f;
            Vector3 byAngles = new Vector3(0.0f, 180.0f, 0.0f);
            Quaternion fromAngle = transform.rotation;
            Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
            for (float t = 0f; t < 1f; t += Time.deltaTime / inTime)
            {
                transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);

                if (t >= 0.5f)
                {
                    _cardSprite.gameObject.SetActive(true);
                    _cardBack.SetActive(false);
                }
                yield return null;
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0.0f, 180.0f, 0.0f));
            _cardSprite.gameObject.SetActive(true);
            _cardBack.SetActive(false);
            yield break;
        }
    }

	public IEnumerator SetCardActiveState(bool state)
	{
		if (state != _isActive)
		{
			_isActive = state;
			SetCardOrder();
		}
		if (state)
		{
			iTween.ColorTo(_cardSprite.gameObject, iTween.Hash("time", 0.3f, "color", Color.white));
			//_cardSprite.color = Color.white;
			yield return null;
		}
		else
		{
			iTween.ColorTo(_cardSprite.gameObject, iTween.Hash("time", 0.3f, "color", new Color(0.2f, 0.2f, 0.2f, 1)));
			//_cardSprite.color = Color.gray;
			yield return null;
		}
	}


    /*
	public void SetCard(int cardId, Sprite cardSprite)
	{
		_cardId = cardId;
		_cardSprite.sprite = cardSprite;

		_cardValue =  2 + cardId % 13;
		if (_cardValue == 14)
		{
			_cardValue = 11;
		}
		else if (_cardValue > 10)
		{
			_cardValue = 10;
		}

		_cardType = (eCardType)(_cardId / 13);
	}

	public void ResetCard()
	{

	}

	public void DealingCard(int index)
	{
		this.transform.rotation = _originalRotation;
		_cardBack.SetActive(true);

		_cardBack.GetComponent<SpriteRenderer>().sortingOrder = index;
		_cardSprite.sortingOrder = index;
	}

	public int Value
	{
		get
		{
			return _cardValue; 
		}
	}

	public bool IsFlipped
	{
		get
		{
			return _isFlipped;
		}
	}

	public override string ToString ()
	{
		return string.Format ("[BJCardController] " + _cardValue + " of " + _cardType);
	}

	public void FlipCard(bool animated, System.Action completionAction)
	{
		if (!_isFlipped)
		{
			_isFlipped = true;
			if (animated)
			{
				System.Action onCompleteAction = ()=>
				{
					if (completionAction != null)
					{
						completionAction();
					}
				};

				System.Action flipToFronAction = ()=>
				{
					_cardBack.SetActive(false);	
					iTween.RotateTo(this.gameObject, iTween.Hash("time", 0.15f, "y", 0.0f,  "easetype" , iTween.EaseType.easeInQuart, "oncompleteAction", onCompleteAction));
					iTween.ScaleTo(this.gameObject, iTween.Hash("time", 0.15f, "x", 1.0f, "y", 1.0f, "easetype" ,iTween.EaseType.easeInQuart));
				};
				iTween.RotateTo(this.gameObject, iTween.Hash("time", 0.15f, "y", -85f, "oncompleteAction", flipToFronAction, "easetype" , iTween.EaseType.easeOutQuart));
				iTween.ScaleTo(this.gameObject, iTween.Hash("time", 0.15f, "x", 1.4f, "y", 1.2f, "easetype" , iTween.EaseType.easeOutQuart));
			}
			else
			{
				_cardBack.SetActive(false);	
				if (completionAction != null)
				{
					completionAction();
				}
			}
		}
	}

    */

}
