using UnityEngine;
using System.Collections;


[System.Serializable]
public class ProductDataModel  {

	[SerializeField]
	public string Type;

	[SerializeField]
	public int Amount;

	public ProductDataModel(Hashtable data)
	{
		if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_PRODUCT_TYPE))
		{
			Type = data[ServerRequestKeys.SERVER_RESPONSE_KEY_PRODUCT_TYPE].ToString();
		}

		if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_PRODUCT_AMOUNT))
		{
			Amount = int.Parse(data[ServerRequestKeys.SERVER_RESPONSE_KEY_PRODUCT_AMOUNT].ToString());
		}

	}
}
