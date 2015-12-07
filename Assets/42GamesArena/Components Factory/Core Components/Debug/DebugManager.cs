using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour {


	[SerializeField]
	private bool _lockAllTiles;

	[SerializeField]
	private bool _unlockAllTiles;

	[SerializeField]
	private bool _infinateCoins;

	[SerializeField]
	private bool _overrideIAP;

	private static DebugManager _instance;

	// Use this for initialization
	void Awake () {
		if (DebugManager._instance == null)
		{
			DebugManager._instance = this;
		}
		else
		{
			Debug.LogError("ERROR - Debug Manager already initialized!");
		}

		if (PlayerPrefsUtil.HasKey("_unlockAllTiles"))
		{
			bool.TryParse(PlayerPrefsUtil.GetString("_unlockAllTiles") , out _unlockAllTiles);
		}

		if (PlayerPrefsUtil.HasKey("_lockAllTiles"))
		{
			bool.TryParse(PlayerPrefsUtil.GetString("_lockAllTiles") , out _lockAllTiles);
		}

		if (PlayerPrefsUtil.HasKey("_overrideIAP"))
		{
			bool.TryParse(PlayerPrefsUtil.GetString("_overrideIAP") , out _overrideIAP);
		}

	}


	private void SaveSettings()
	{
		PlayerPrefsUtil.SetString("_unlockAllTiles", _unlockAllTiles.ToString());
		PlayerPrefsUtil.SetString("_lockAllTiles", _lockAllTiles.ToString());
		PlayerPrefsUtil.SetString("_overrideIAP", _overrideIAP.ToString());
	}

	#region Public

	public static DebugManager Instance()
	{
		if (DebugManager._instance != null)
		{
			return DebugManager._instance;
		}

		throw new System.Exception("Debug Manager could not be found");
	
	}


	public static bool LockAllTiles
	{
		get
		{
			return DebugManager.Instance()._lockAllTiles;
		}
		set
		{
			DebugManager.Instance()._lockAllTiles = value;
			if (DebugManager.Instance()._lockAllTiles)
			{
				DebugManager.Instance()._unlockAllTiles = false;
			}

			DebugManager.Instance().SaveSettings();
		}
	}

	public static bool UnLockAllTiles
	{
		get
		{
			return DebugManager.Instance()._unlockAllTiles;
		}
		set
		{
			DebugManager.Instance()._unlockAllTiles = value;
			if (DebugManager.Instance()._unlockAllTiles)
			{
				DebugManager.Instance()._lockAllTiles = false;
			}
			DebugManager.Instance().SaveSettings();
		}
	}

	public static bool InfinateCoins
	{
		get
		{
			return DebugManager.Instance()._infinateCoins;
		}
		set
		{
			DebugManager.Instance()._infinateCoins = value;
			DebugManager.Instance().SaveSettings();
		}
	}

	public static void PlayVideoAd()
	{

	}

	public static void ClearUserCache()
	{
		IAccount accountManager = ComponentFactory.GetAComponent<IAccount>();
		if (accountManager != null)
		{
			accountManager.ClearUserCache();
		}
	}

	public static void ClearInventory()
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		if (inventoryManager != null)
		{

		}
	}

	public static bool OverrideIAP
	{
		get
		{
			return DebugManager.Instance()._overrideIAP;
		}
		set
		{
			DebugManager.Instance()._overrideIAP = value;
			DebugManager.Instance().SaveSettings();
		}
	}

	#endregion

}


public class AppReloader :MonoBehaviour
{
	public static void ReloadApp()
	{
		GameObject appReloaderContainer = new GameObject();
		appReloaderContainer.AddComponent<AppReloader>();
	}

	public void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		StartCoroutine(ReloadAppCorutine());
	}

	IEnumerator ReloadAppCorutine()
	{

		AssetManager.instance.UnloadAllAssets();
		AssetManager.instance = null;

		yield return null;

		GameObject[] objects = FindObjectsOfType<GameObject>() as GameObject[];


		yield return null;

		for (int i=0; i< objects.Length; i++)
		{
			if (objects[i] != null && objects[i] != null && objects[i] != this.gameObject)
			{
				Destroy(objects[i]);
				yield return null;
			}
		}
		yield return null;

		Application.LoadLevel(0);


		Destroy(this.gameObject);
	}
}

