using UnityEngine;
using System.Collections;

[System.Serializable]
public class PriceDataModel  {

	[SerializeField]
	public ePriceAction ActionType;

	[SerializeField]
	public int Value;

	[SerializeField]
	public int GameId;

	[SerializeField]
	public string Link;

	[SerializeField]
	public string StoreLink;

	[SerializeField]
	public string PriceString;

	public PriceDataModel(Hashtable data)
	{
		if (data != null)
		{
			ActionType = (ePriceAction)System.Enum.Parse(typeof(ePriceAction), data[ServerRequestKeys.SERVER_RESPONSE_KEY_PRICE_ACTION].ToString());
			Value = int.Parse(data[ServerRequestKeys.SERVER_RESPONSE_KEY_PRICE_VALUE].ToString());
			if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_PRICE_LINK))
			{
				Link = data[ServerRequestKeys.SERVER_RESPONSE_KEY_PRICE_LINK].ToString();
			}

			if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_PRICE_STORE_LINK))
			{
				StoreLink = data[ServerRequestKeys.SERVER_RESPONSE_KEY_PRICE_STORE_LINK].ToString();
			}
		}
	}
	
}
