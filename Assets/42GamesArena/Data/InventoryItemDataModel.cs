using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItemDataModel  {

	[SerializeField]
	public int Amount;

	[SerializeField]
	public string ItemType;


	public InventoryItemDataModel(Hashtable data)
	{
		if (data != null)
		{
			Amount = int.Parse(data[ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_ITEM_AMOUNT].ToString());
			ItemType = data[ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_ITEM_TYPE].ToString();
		}
	}

	public override string ToString ()
	{
		return string.Format ("[InventoryItemDataModel] ItemType: {0}, Amount: {1}", ItemType, Amount);
	}
}
