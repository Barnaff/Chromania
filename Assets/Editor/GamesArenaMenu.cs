using UnityEngine;
using System.Collections;
using UnityEditor;

public class GamesArenaMenu {

	public const string MENU_PATH = "42 Games Arena/Options/";

	[MenuItem(GamesArenaMenu.MENU_PATH + "Clear Local Account")]
	static void ClearLocalAccount()
	{
		PlayerPrefsUtil.Delete(ServerRequestsManager.USER_TOKEN);
	}

	[MenuItem(GamesArenaMenu.MENU_PATH + "Clear PlayerPrefs")]
	static void ClearPlayerPrefs()
	{
		PlayerPrefsUtil.DeleteAll();
	}

}
