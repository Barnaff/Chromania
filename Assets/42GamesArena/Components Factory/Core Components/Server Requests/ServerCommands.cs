using UnityEngine;
using System.Collections;

public class ServerCommands {
	
	#region User

	public const string SERVER_COMMAND_USER_UPDATE_DEVICE = "user/updateDevice";
	public const string SERVER_COMMAND_USER_LOGIN = "user/login";
	public const string SERVER_COMMAND_USER_FACEBOOKLOGIN = "user/facebookLogin";
	public const string SERVER_COMMAND_USER_LOGOUT = "user/logout";

	#endregion



	#region Portal

	// Depricated
	//public const string SERVER_COMMAND_GET_PORTAL = "portal/getPortal";

	public const string SERVER_COMMAND_GET_LOBBY = "portal/getLobby";


	#endregion



	#region Ads

	public const string SERVER_COMMAND_GET_ADLIST = "ad/getAdList";
	public const string SERVER_COMMAND_GET_INCENT = "ad/getIncent";
	public const string SERVER_COMMAND_GET_INCENT_LIST = "ad/getIncentList";

	#endregion


	#region Inventory as Shop

	public const string SERVER_COMMAND_GET_INVENTORY = "treasury/getInventory";
	public const string SERVER_COMMAND_CONSUME_FROM_INVENTORY = "treasury/consumeFromInventory";
	public const string SERVER_COMMAND_ADD_INVENTORY_ITEM = "treasury/addToInventory";
	public const string SERVER_COMMAND_BUY_SHOP_ITEM = "treasury/buyShopItem";
	public const string SERVER_COMMAND_VERIFY_PURCHASE = "treasury/verifyPurchase";

	#endregion


}
