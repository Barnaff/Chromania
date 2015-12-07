using UnityEngine;
using System.Collections;

[RequireComponent(typeof(INetwork))]
public class ServerRequestsManager : MonoBehaviour,  IServerRequests {

	#region Local Keys

	public const string USER_TOKEN = "authToken";

	#endregion


	#region Private Properties

	private string _userToken = null;
	private bool _hasLocaluser = false;

	private INetwork _networkManager;

	#endregion


	#region Initialize

	void Awake()
	{
		_networkManager = ComponentFactory.GetAComponent<INetwork>() as INetwork;
		if (_networkManager == null)
		{
			Debug.LogError("ERROR - cant find network manager");
		}
	}

	#endregion
	

	#region Private
	
	public void SendServerRequest(string command, Hashtable data, System.Action <Hashtable> completionAction, System.Action <ServerError> failAction, bool authonticate = true)
	{
		if (authonticate && HasLocalUser)
		{
			if (data == null)
			{
				data = new Hashtable();
			}
			data.Add(ServerRequestKeys.SERVER_KEY_AUTH_TOKEN, _userToken);
		}
		_networkManager.PostServerCommand(command, data, completionAction, failAction);
	}

	public void ClearLocalUser()
	{
		_hasLocaluser = false;
		_userToken = null;
	}
	
	#endregion


	#region Private

	private bool HasLocalUser
	{
		get
		{
			if (_hasLocaluser)
			{
				return _hasLocaluser;
			}
			else
			{
				if (PlayerPrefsUtil.HasKey(USER_TOKEN))
				{
					_userToken = PlayerPrefsUtil.GetString(USER_TOKEN);
					_hasLocaluser = true;
					return _hasLocaluser;
				}
				else
				{
					return false;
				}
			}
		}
	}

	#endregion



}
