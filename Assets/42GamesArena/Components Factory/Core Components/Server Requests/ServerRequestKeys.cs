public class ServerRequestKeys  {


	#region Requests Keys

	// general
	public const string SERVER_KEY_AUTH_TOKEN = "authToken";
	public const string SERVER_KEY_ID	= "id";
	public const string SERVER_KEY_OS_TYPE = "osType";

	// account
	public const string SERVER_KEY_FACEBOOK_ID = "facebookId";
	public const string SERVER_KEY_DEVICE_DETAILS = "deviceDetails";
	public const string SERVER_KEY_DEVICE_ID = "deviceId";
	public const string SERVER_KEY_DEVICE_TYPE = "deviceType";
	public const string SERVER_KEY_DEVICE_MODEL = "deviceModel";
	public const string SERVER_KEY_APPLICATION_VERSION = "applicationVersion";
	public const string SERVER_KEY_OS_VERSION = "osVersion";
	public const string SERVER_KEY_PN_TOKEN = "pnToken";
	public const string SERVER_KEY_PN_TYPE = "pnType";
	public const string SERVER_KEY_TIME_ZONE = "timeZone";
	public const string SERVER_KEY_PORTAL_KEY = "portalKey";
	public const string SERVER_KEY_FACEBOOK_TOKEN = "facebookToken";

	// portal
	public const string SERVER_KEY_ISO_CODE = "isoCode";
	public const string SERVER_KEY_PORTAL_ID = "portalId";

	//shop item
	public const string SERVER_KEY_SHOP_ITEM_ID = "shopItemId";
	public const string SERVER_KEY_PURCHASE_DATA = "purchaseData";


	#endregion


	#region Types

	// device Types
	public const string SERVER_KEY_DEVICE_TYPE_PHONE = "PHONE";
	public const string SERVER_KEY_DEVICE_TYPE_TABLET = "TABLET";
	public const string SERVER_KEY_DEVICE_TYPE_WEB = "WEB";
	public const string SERVER_KEY_DEVICE_TYPE_PC = "PC";
	public const string SERVER_KEY_PN_TYPE_DEBUG = "DEBUG";
	public const string SERVER_KEY_PN_TYPE_RELEASE = "RELEASE";
	public const string SERVER_KEY_OS_TYPE_IOS = "IOS";
	public const string SERVER_KEY_OS_TYPE_ANDROID = "ANDROID";

	// tile types
	public const string SERVER_TILE_TYPE_GAME = "GAME";
	public const string SERVER_TILE_TYPE_AD = "AD";

	// ads
	public const string SERVER_AD_SHOP_ITEM = "shopItem";
	public const string SERVER_AD_INCENTIVE = "incentive";

	
	#endregion


	#region Response Keys

	// general
	public const string SERVER_RESPONSE_KEY_AUTH_TOKEN = "authToken";
	public const string SERVER_RESPONSE_KEY_ID = "id";
	public const string SERVER_RESPONSE_KEY_NAME = "name";


	// portal
	public const string SERVER_RESPONSE_KEY_LOBBY = "lobby";
	public const string SERVER_RESPONSE_KEY_LOBBY_BUNDLE = "lobbyBundle";
	public const string SERVER_RESPONSE_KEY_LOBBY_TITLE = "lobbyTitle";
	public const string SERVER_RESPONSE_KEY_LOBBY_SCENE = "lobbyScene";
	public const string SERVER_RESPONSE_KEY_LOCAL_CODE = "localCode";
	public const string SERVER_RESPONSE_KEY_USE_LOBBY_SCENE = "useLobbyScene";
	public const string SERVER_RESPONSE_KEY_ASSET_BUNDLE_KEY = "assetBundleKey";
	public const string SERVER_RESPONSE_KEY_LOBBY_SHOP_ITEMS = "lobbyShopItems";
	public const string SERVER_RESPONSE_KEY_ENABLE_HEARTS = "enableHearts";
	public const string SERVER_RESPONSE_KEY_ENABLE_STARS = "enableStars";
	public const string SERVER_RESPONSE_KEY_TERMS_OF_USE_URL = "termsOfUse";
	public const string SERVER_RESPONSE_KEYPRIVACY_POLICY_URL = "privacyPolicy";
	public const string SERVER_RESPONSE_KEY_TILES = "tiles";


	// tiles
	public const string SERVER_RESPONSE_KEY_TILE_TYPE = "type";
	public const string SERVER_RESPONSE_KEY_TILE_ID = "id";
	public const string SERVER_RESPONSE_KEY_TILE_SIZE = "tileSize";
	public const string SERVER_RESPONSE_KEY_TILE_ORDER = "tileOrder";
	public const string SERVER_RESPONSE_KEY_TILE_GAME_DEFENITION = "gameDefinition";
	public const string SERVER_RESPONSE_KEY_TILE_SHOP_ITEMS = "tileShopItem";


	// game defenition
	public const string SERVER_RESPONSE_KEY_GAME_DEFENITION_ID = "id";
	public const string SERVER_RESPONSE_KEY_GAME_DEFENITION_NAME = "name";
	public const string SERVER_RESPONSE_KEY_GAME_DEFENITION_BUNDLE_NAME = "bundleName";
	public const string SERVER_RESPONSE_KEY_GAME_DEFENITION_SCENE_NAME = "sceneName";
	public const string SERVER_RESPONSE_KEY_GAME_DEFENITION_ICON_NAME = "iconName";
	public const string SERVER_RESPONSE_KEY_GAME_DEFENITION_GAME_VARIABLES = "gameVariables";


	// ads
	public const string SERVER_RESPONSE_KEY_ADS_LIST = "ads";
	public const string SERVER_RESPONSE_KEY_AD_IMAGE_URL = "picUrl";
	public const string SERVER_RESPONSE_KEY_AD_IMAGE_1_URL = "picUrl1";
	public const string SERVER_RESPONSE_KEY_AD_IMAGE_2_URL = "picUrl2";
	public const string SERVER_RESPONSE_KEY_AD_IMAGE_3_URL = "picUrl3";
	public const string SERVER_RESPONSE_KEY_AD_IMAGE_4_URL = "picUrl4";
	public const string SERVER_RESPONSE_KEY_AD_URL = "url";
	public const string SERVER_RESPONSE_KEY_AD_OBJECT = "ad";
	public const string SERVER_RESPONSE_KEY_AD_DISPLAY_NAME = "adDisplayName";


	// Inventory
	public const string SERVER_RESPONSE_KEY_INVENTORY_ITEM = "inventoryItem";
	public const string SERVER_RESPONSE_KEY_INVENTORY_ITEM_TYPE = "type";
	public const string SERVER_RESPONSE_KEY_INVENTORY_ITEM_AMOUNT = "amount";
	public const string SERVER_RESPONSE_KEY_INVENTORY_CONSUME_ITEM_TYPE = "itemType";
	public const string SERVER_RESPONSE_KEY_INVENTORY_CONSUME_ITEM_AMOUNT = "amount";

	public const string SERVER_RESPONSE_KEY_INVENTORY_ADD_ITEM_TYPE = "itemType";
	public const string SERVER_RESPONSE_KEY_INVENTORY_ADD_ITEM_AMOUNT = "amount";

	public const string SERVER_RESPONSE_KEY_INVENTORY_PRODUCT_RECIVED_TYPE = "type";
	public const string SERVER_RESPONSE_KEY_INVENTORY_PRODUCT_RECIVED_AMOUNT = "amount";


	// shop item
	public const string SERVER_RESPONSE_KEY_SHOP_ITEM_ID = "id";
	public const string SERVER_RESPONSE_KEY_SHOP_ITEM_PRICE = "price";
	public const string SERVER_RESPONSE_KEY_SHOP_ITEM_PRODUCT = "product";
	public const string SERVER_RESPONSE_KEY_SHOP_ITEM_PGENERAL_SHOP_ITEM = "generalShopItem";


	// price data
	public const string SERVER_RESPONSE_KEY_PRICE_ACTION = "priceAction";
	public const string SERVER_RESPONSE_KEY_PRICE_VALUE = "value";
	public const string SERVER_RESPONSE_KEY_PRICE_LINK = "link";
	public const string SERVER_RESPONSE_KEY_PRICE_STORE_LINK = "storeLink";


	// product data
	public const string SERVER_RESPONSE_KEY_PRODUCT_TYPE = "type";
	public const string SERVER_RESPONSE_KEY_PRODUCT_AMOUNT = "amount";


	#endregion
}
