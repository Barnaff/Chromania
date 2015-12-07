using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(NetworkManager))]
public class NetworkManagerInspector : Editor {

	private Dictionary<string, string> _storedURLs;
	private string[] _urlKeys;
	private int _selectedServerIndex = -1;

	private string STORED_URLS_KEY = "storedURLS";

	private bool _isManaging = false;

	private string _newServerName = "";
	private string _newServerURL = "";

	private string _editingKey = "";
	private bool _isAddingNewServer = false;

	private bool _isEditingSettings = false;



	public override void OnInspectorGUI()
	{
		if (_selectedServerIndex == -1)
		{
			if (EditorPrefs.HasKey("_selectedServerIndex"))
			{
				_selectedServerIndex = EditorPrefs.GetInt("_selectedServerIndex");
			}
		}
		if (_storedURLs == null)
		{
			LoadStoredURLS();
		}

		NetworkManager networkManager = (NetworkManager)target;

		GUILayout.BeginVertical("Box");
		string error = "";
		if (_urlKeys != null && _urlKeys.Length > 0)
		{
			if (_selectedServerIndex >= _urlKeys.Length)
			{
				Debug.Log("no selected keys");
				_selectedServerIndex = 0;
			}

			if (_storedURLs.ContainsValue(networkManager.ServerURL))
			{
				foreach (string key in _storedURLs.Keys)
				{
					if (_storedURLs[key] == networkManager.ServerURL)
					{
						for (int i=0; i< _urlKeys.Length; i++)
						{
							if (key == _urlKeys[i])
							{
								_selectedServerIndex = i;
							}
						}
					}
				}
			}
			int lastIndex = _selectedServerIndex;
			_selectedServerIndex = EditorGUILayout.Popup("Select Server", _selectedServerIndex, _urlKeys);

			if (_storedURLs.ContainsKey(_urlKeys[_selectedServerIndex]))
			{
				string serverURL = _storedURLs[_urlKeys[_selectedServerIndex]];
				networkManager.ServerURL = serverURL;
				EditorGUILayout.HelpBox("server URL: " + networkManager.ServerURL, MessageType.Info);
				error = "";
			}
			else
			{
				error = "Invalid Server";
			}

			if (lastIndex != _selectedServerIndex)
			{
				EditorPrefs.SetInt("_selectedServerIndex", _selectedServerIndex);
				SaveStoredURLs();
			}


		}
		else
		{
			error = "No servers avalable!, Please add a server";
		}

		if (!string.IsNullOrEmpty(error))
		{
			EditorGUILayout.HelpBox(error, MessageType.Error);
		}

		GUILayout.EndVertical();


		if (!_isManaging)
		{
			if (GUILayout.Button("Manage Servers"))
			{
				_isManaging = true;

			}
		}
		else
		{
			GUILayout.BeginVertical("Box");

			GUILayout.BeginHorizontal();

			GUILayout.Label("Manage Servers");
			if (GUILayout.Button("X", GUILayout.Width(20)))
			{
				_isManaging = false;
			}

			GUILayout.EndHorizontal();

			foreach (string serverName in _storedURLs.Keys)
			{
				GUILayout.BeginHorizontal("Box");

				if (serverName == _editingKey)
				{
					_newServerName = EditorGUILayout.TextField(_newServerName);
					_newServerURL = EditorGUILayout.TextField(_newServerURL);

					if (GUILayout.Button("V", GUILayout.Width(20)))
					{
						Debug.Log("removing: " + serverName);
						_storedURLs.Remove(serverName);
						_storedURLs.Add(_newServerName, _newServerURL);
						SaveStoredURLs();
						LoadStoredURLS();
						_editingKey = "";
						return;
					}

					if (GUILayout.Button("X", GUILayout.Width(20)))
					{
						_storedURLs.Remove(serverName);
						SaveStoredURLs();
						LoadStoredURLS();
						_editingKey = "";
						return;
					}
				}
				else
				{ 
					GUILayout.Label(serverName);
					GUILayout.Label(_storedURLs[serverName]);
					
					if (GUILayout.Button("E", GUILayout.Width(20)))
					{
						_newServerName = serverName;
						_newServerURL = _storedURLs[serverName];
						_editingKey = serverName;
						_isAddingNewServer = false;
					}
				}

				GUILayout.EndHorizontal();
			}

			GUILayout.Space(10);

			GUILayout.BeginVertical("Box");

			if (_isAddingNewServer)
			{
				_newServerName = GUILayout.TextField(_newServerName);
				_newServerURL = GUILayout.TextField(_newServerURL);

				GUILayout.BeginHorizontal();

				if (GUILayout.Button("Save Server"))
				{
				
					AddServer(_newServerName, _newServerURL);
					SaveStoredURLs();
					_isAddingNewServer = false;
				}

				if (GUILayout.Button("Cancel"))
				{
					_isAddingNewServer = false;
				}

				GUILayout.EndHorizontal();
			}
			else
			{
				if (GUILayout.Button("Add New Server"))
				{
					_newServerName = "Server Name";
					_newServerURL = "Server URL";
					_isAddingNewServer = true;
				}
			}

			GUILayout.EndVertical();

			GUILayout.Space(10);

			if (GUILayout.Button("Delete All Servers"))
			{
				EditorPrefs.DeleteKey(STORED_URLS_KEY);
				_isManaging = false;
				LoadStoredURLS();
			}

			GUILayout.EndHorizontal();
		}

		GUILayout.Space(10);

		if (_isEditingSettings)
		{
			GUILayout.BeginVertical("Box");

			GUILayout.BeginHorizontal();
			GUILayout.Label("Advanced Settings");
			if (GUILayout.Button("X", GUILayout.Width(20)))
			{
				_isEditingSettings = false;
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginVertical("Box");

			networkManager.EnableDebug = GUILayout.Toggle(networkManager.EnableDebug, "Enable Debug Logs");
			networkManager.CacheRequests = GUILayout.Toggle(networkManager.CacheRequests, "Cache Requests");
			if (networkManager.CacheRequests)
			{
				networkManager.RetryCount = EditorGUILayout.IntField("Retry Count", networkManager.RetryCount);
				networkManager.RetryTimer = EditorGUILayout.FloatField("Retry Timer", networkManager.RetryTimer);
			}

			networkManager.ResultString = EditorGUILayout.TextField("Result String", networkManager.ResultString);
			networkManager.ErrorString = EditorGUILayout.TextField("Error String", networkManager.ErrorString);

			GUILayout.EndVertical();

			GUILayout.EndVertical();
		}
		else
		{
			if (GUILayout.Button("Advanced Settings"))
			{
				_isEditingSettings = true;
			}
		}

		GUILayout.Space(10);

		base.DrawDefaultInspector();
	}

	private void AddServer(string serverName, string serverURL)
	{
		string stordesServerString = "";
		if (EditorPrefs.HasKey(STORED_URLS_KEY))
		{
			stordesServerString = EditorPrefs.GetString(STORED_URLS_KEY);
		}
		if (stordesServerString.Length > 0)
		{
			stordesServerString += ",";
		}
		stordesServerString += serverName + "," + serverURL;
		EditorPrefs.SetString(STORED_URLS_KEY, stordesServerString);
		
		LoadStoredURLS();
	}

	private void LoadStoredURLS()
	{
		_storedURLs = new Dictionary<string, string>();
		_urlKeys = null;
		if (EditorPrefs.HasKey(STORED_URLS_KEY))
		{
			string[] fields = EditorPrefs.GetString(STORED_URLS_KEY).Split(","[0]);

			if (fields.Length % 2 != 0)
			{
				Debug.LogError("ERROR - Network manager fields list is incorrect!");
				return;
			}

			_urlKeys = new string[fields.Length / 2];
			int keyCount = 0;
			for (int i=0; i< fields.Length; i += 2)
			{
				if (_storedURLs.ContainsKey(fields[i]))
				{
					Debug.LogError("ERROR = There is a duplicate server!");
				}
				else
				{
					_storedURLs.Add(fields[i], fields[i+1]);
					_urlKeys[keyCount] = fields[i];
					keyCount++;
				}
			}
		}
		_editingKey = "";
	}

	private void SaveStoredURLs()
	{
		string storedServerString = "";
		foreach (string serverName in _storedURLs.Keys)
		{
			storedServerString += serverName + "," + _storedURLs[serverName] + ",";
		}
		
		storedServerString = storedServerString.Remove(storedServerString.Length - 1, 1);
		
		Debug.Log("saved: " + storedServerString);
		EditorPrefs.SetString(STORED_URLS_KEY, storedServerString);
	}
	
}


