using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AdDataModel  
{
	[SerializeField]
	public int AdId;

	[SerializeField]
	public AdPresentationType PresenetationType;

	[SerializeField]
	public string AdImageURL;

	[SerializeField]
	public string AdTargetURL;

	[SerializeField]
	public string AdDisplayName;

	public enum eAdImageSizeType
	{
		SIZE1,
		SIZE2,
		SIZE3,
		SIZE4,
	}

	[SerializeField]
	private Dictionary<eAdImageSizeType, string> _imagesURLForSizes;
	

	public AdDataModel(Hashtable data)
	{
		AdImageURL = data[ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_1_URL].ToString();
		AdTargetURL = data[ServerRequestKeys.SERVER_RESPONSE_KEY_AD_URL].ToString();

		_imagesURLForSizes = new Dictionary<eAdImageSizeType, string>();

		if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_1_URL))
		{
			_imagesURLForSizes.Add(eAdImageSizeType.SIZE1, data[ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_1_URL].ToString());
		}

		if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_2_URL))
		{
			_imagesURLForSizes.Add(eAdImageSizeType.SIZE2, data[ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_2_URL].ToString());
		}

		if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_3_URL))
		{
			_imagesURLForSizes.Add(eAdImageSizeType.SIZE3, data[ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_3_URL].ToString());
		}

		if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_4_URL))
		{
			_imagesURLForSizes.Add(eAdImageSizeType.SIZE4, data[ServerRequestKeys.SERVER_RESPONSE_KEY_AD_IMAGE_4_URL].ToString());
		}

		if (data.Contains(ServerRequestKeys.SERVER_RESPONSE_KEY_AD_DISPLAY_NAME))
		{
			AdDisplayName = data[ServerRequestKeys.SERVER_RESPONSE_KEY_AD_DISPLAY_NAME].ToString();
		}
	} 

	public string AdImageForSize(eAdImageSizeType sizeType)
	{
		if (_imagesURLForSizes.ContainsKey(sizeType))
		{
			return _imagesURLForSizes[sizeType];
		}
		Debug.LogError("ERROR - No ad image for size: " + sizeType);
		return null;
	}
}
