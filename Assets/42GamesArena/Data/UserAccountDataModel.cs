using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UserAccountDataModel  {

	#region Account

	[SerializeField]
	public int UserID;

	[SerializeField]
	public string UserToken;

	#endregion


	#region Social

	[SerializeField]
	public string UserName;

	[SerializeField]
	public string FacebookID;

	[SerializeField]
	public string UserImageURL;

	#endregion


	#region Inventory

	[SerializeField]
	public int CoinsCount;
	
	[SerializeField]
	List<InventoryItemDataModel> InventoryItems;

	#endregion
	

}
