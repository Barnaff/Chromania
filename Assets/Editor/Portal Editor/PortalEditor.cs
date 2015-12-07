using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class PortalEditor : EditorWindow {

	private bool _isLogedIn;

	private string _authtoken = "";

	private string _serverURL = "";

	private const string SERVER_URL_KEY = "serverURLKey";

	private const string AUTH_TOKEN_KEY = "authTikenKey";


	private LobbyDataModel _portalData = null;

	private int _portalId;

	public const string MENU_PATH = "42 Games Arena/";

	private NetworkManager _networkManager;



	private enum eEditMode
	{
		Tiles, 
		Shop,
		Ads,
	}

	private eEditMode _editMode;

	// Add menu named "My Window" to the Window menu
	[MenuItem (MENU_PATH + "Portal Editor")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		PortalEditor window = (PortalEditor)EditorWindow.GetWindow (typeof (PortalEditor));
		window.Show();
		window.InitEditor();


	}

	private void InitEditor()
	{
		if (EditorPrefs.HasKey(SERVER_URL_KEY))
		{
			_serverURL = EditorPrefs.GetString(SERVER_URL_KEY);
		}

		if (EditorPrefs.HasKey(AUTH_TOKEN_KEY))
		{
			_authtoken = EditorPrefs.GetString(AUTH_TOKEN_KEY);
		}

		_portalData = null;
	}
	
	void OnGUI () {
	
		if (_isLogedIn)
		{
			DisplayEditor();
		}
		else
		{
			DisplayLogin();
		}

		if (_editMode == eEditMode.Tiles)
		{
			
		}


	}


	private void DisplayLogin()
	{
		float loginBoxWidth = 400; 

		GUILayout.BeginArea(new Rect(position.width * 0.5f - (loginBoxWidth * 0.5f) , position.height * 0.5f, loginBoxWidth, 100));

		GUILayout.Box("Login");

		_serverURL = EditorGUILayout.TextField("Server URL: ", _serverURL);

		if (GUILayout.Button("Login"))
		{
			PrformServerLogin();
		}


		GUILayout.EndArea();
	}


	private void DisplayEditor()
	{
		if (_portalData != null)
		{
			DisplayPortalEditor();
		}
		else
		{
			DisplayPortalSelection();
		}
	}

	private void DisplayPortalSelection()
	{


		float portalSelectionWidth = 400; 
		
		GUILayout.BeginArea(new Rect(position.width * 0.5f - (portalSelectionWidth * 0.5f) , position.height * 0.5f, portalSelectionWidth, 300));
		
		GUILayout.BeginVertical("Box");

		EditorGUILayout.LabelField("Select Portal");

		_portalId = EditorGUILayout.IntField("Portal Id", _portalId);

		if (GUILayout.Button("Load Portal"))
		{
			LoadPortal(_portalId);
		}

		GUILayout.EndVertical();
		
		
		GUILayout.EndArea();
	}

	private void DisplayPortalEditor()
	{
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal("Box");

		if (GUILayout.Button("Edit Tiles"))
		{
			_editMode = eEditMode.Tiles;
		}

		if (GUILayout.Button("Edit Shop Items"))
		{
			_editMode = eEditMode.Shop;
		}

		if (GUILayout.Button("Edit Ads"))
		{
			_editMode = eEditMode.Ads;
		}

		GUILayout.Space(position.width * 0.2f);

		if (GUILayout.Button("Reload Portal"))
		{

		}

		if (GUILayout.Button("Save Portal"))
		{
			
		}

		if (GUILayout.Button("Logout"))
		{
			Logout();
		}

		GUILayout.EndHorizontal();


		GUILayout.EndVertical();
	}



	#region Server Calls

	private void PrformServerLogin()
	{
		string command = ServerCommands.SERVER_COMMAND_USER_LOGIN;
		Hashtable data = new Hashtable();

		Hashtable deviceDetails = new Hashtable();
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_DEVICE_ID, SystemInfo.deviceUniqueIdentifier);
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_DEVICE_TYPE, ServerRequestKeys.SERVER_KEY_DEVICE_TYPE_PHONE);
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_DEVICE_MODEL, SystemInfo.deviceModel);
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_APPLICATION_VERSION, Application.version);
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_OS_VERSION, SystemInfo.operatingSystem);
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_PN_TOKEN, "");
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_PN_TYPE, ServerRequestKeys.SERVER_KEY_PN_TYPE_DEBUG);
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_OS_TYPE, ServerRequestKeys.SERVER_KEY_OS_TYPE_IOS);
		deviceDetails.Add(ServerRequestKeys.SERVER_KEY_TIME_ZONE, System.TimeZone.CurrentTimeZone.ToString());

		data.Add(ServerRequestKeys.SERVER_KEY_DEVICE_DETAILS, deviceDetails);
		
		SendServerCommand(command, data, (response)=>
		{
			if (response.ContainsKey("authToken"))
			{
				_authtoken = response["authToken"].ToString();
				EditorPrefs.SetString(AUTH_TOKEN_KEY, _authtoken);
				_portalData = null;
				_isLogedIn = true;

				Debug.Log("got auth token: " + _authtoken);

				this.Repaint();
			}
		
			EditorPrefs.SetString(SERVER_URL_KEY, _serverURL);
			
		}, (error) =>
		{
			Debug.LogError(error.ErrorDescription);

		});

	}

	private void LoadPortal(int portalId)
	{
//		string command = "portal/getLobby";  
//
//		Hashtable data = new Hashtable();
//		data.Add("portalId", portalId);
//
//		if (!string.IsNullOrEmpty(_authtoken))
//		{
//			data.Add("authToken", _authtoken);
//		}

//		SendServerCommand(command, data, (response)=>
//		                  {
//
//			Hashtable portalData = response[ServerRequestKeys.SERVER_RESPONSE_KEY_LOBBY] as Hashtable;
//
//
//			_portalData = new LobbyDataModel();
//			_portalData.LobbyName = portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_LOBBY_TITLE].ToString();
//			_portalData.Version = float.Parse(portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_VERSION].ToString());
//			
//			ArrayList tiles = portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_TILES] as ArrayList;
//			
//			List<TileDataModelAbstract> tilesList = new List<TileDataModelAbstract>();
//			foreach (Hashtable tileData in tiles)
//			{
//				GameTileDataModel tile = new GameTileDataModel(tileData);
//				tilesList.Add(tile);
//			}
//			
//			_portalData.LobbyTilesList = tilesList;
//
//			this.Repaint();
//
//		}, (error) =>
//		{
//
//		});


	}

	private void Logout()
	{
		_isLogedIn = false;
	}

	#endregion



	#region Server Interactions

	private void SendServerCommand(string command, Hashtable data, System.Action<Hashtable> sucsessAction, System.Action<ServerError> failAction)
	{
	
		EditorApplication.isPlaying = true;
		_networkManager = GameObject.FindObjectOfType<NetworkManager>();
		if (_networkManager == null)
		{
			GameObject networkManagerContainer = new GameObject();
			DontDestroyOnLoad(networkManagerContainer);
			_networkManager = networkManagerContainer.AddComponent<NetworkManager>();
		}

		_networkManager.ServerURL = _serverURL;

		_networkManager.PostServerCommand(command, data, sucsessAction, failAction);




	}
	

	#endregion

}
