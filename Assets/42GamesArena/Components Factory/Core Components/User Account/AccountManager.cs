using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IServerRequests))]
public class AccountManager : MonoBehaviour, IAccount {
	
	private IServerRequests _serverRequestsManager;

	private const string AUTH_TOKEN_KEY = "authToken";

	#region IAccount Implelemntation

	public void Login(System.Action completionAction)
	{
		string command = ServerCommands.SERVER_COMMAND_USER_LOGIN;
		Hashtable data = new Hashtable();

		data.Add(ServerRequestKeys.SERVER_KEY_DEVICE_DETAILS, GetDeviceDetails());

		string portalIdentifier = ComponentFactory.GetAComponent<IPortal>().PortalIdentifier;
		if (portalIdentifier != null)
		{
			data.Add(ServerRequestKeys.SERVER_KEY_PORTAL_KEY, portalIdentifier);
		}
		else
		{
			throw new System.Exception("ERROR - Cannot get portal identifier, cant perfotm login");
		}

		ServerRequests.SendServerRequest(command, data, (response)=>
		{

			string authToken = response[ServerRequestKeys.SERVER_RESPONSE_KEY_AUTH_TOKEN].ToString();
			StoreUserToken(authToken);
			if (completionAction != null)
			{
				completionAction();
			}

		}, (error) =>
		{
			Debug.LogError(error.ErrorDescription);
			if (completionAction != null)
			{
				completionAction();
			}
		}, false);
	}

	public void UpdateDevice(System.Action completionAction)
	{
		string command = ServerCommands.SERVER_COMMAND_USER_UPDATE_DEVICE;

		Hashtable data = new Hashtable();

		data.Add(ServerRequestKeys.SERVER_KEY_DEVICE_DETAILS, GetDeviceDetails());
		
		ServerRequests.SendServerRequest(command, data, (response)=>
		{
			if (completionAction != null)
			{
				completionAction();
			}

		}, (error) =>
		{
			Debug.LogError(error.ErrorDescription);

			if (error.ErrorCode == 703)
			{
				Debug.Log("ERROR_INVALID_AUTH_TOKEN  - Reseting the user on this device");
				PlayerPrefsUtil.DeleteKey(AUTH_TOKEN_KEY);
				IServerRequests serverRequestsManager = ComponentFactory.GetAComponent<IServerRequests>();
				if (serverRequestsManager != null)
				{
					serverRequestsManager.ClearLocalUser();
				}

				Login(completionAction);
				return;
			}

			if (completionAction != null)
			{
				completionAction();
			}
		});

	}

	public void Logout(System.Action completionAction)
	{
		string command = ServerCommands.SERVER_COMMAND_USER_LOGOUT;
		
		Hashtable data = new Hashtable();

		ServerRequests.SendServerRequest(command, data, (response)=>
		                                         {
			
			Debug.Log("Loged out");
			if (completionAction != null)
			{
				completionAction();
			}

		}, (error) =>
		{
			Debug.LogError(error.ErrorDescription);
			if (completionAction != null)
			{
				completionAction();
			}
		});
	}

	public void ClearUserCache()
	{
		PlayerPrefsUtil.Delete(AUTH_TOKEN_KEY);
	}

	public bool HasLocalUser 
	{ 
		get
		{
			bool hasLocalUser = (PlayerPrefsUtil.HasKey(AUTH_TOKEN_KEY) && !string.IsNullOrEmpty(PlayerPrefsUtil.GetString(AUTH_TOKEN_KEY)));
			return hasLocalUser;
		}
	}

	public void FacebookLogin(string acsessToken, System.Action completionAction)
	{
		string command = ServerCommands.SERVER_COMMAND_USER_FACEBOOKLOGIN;
		Hashtable data = new Hashtable();
		
		if (!string.IsNullOrEmpty(acsessToken))
		{
			data.Add(ServerRequestKeys.SERVER_KEY_FACEBOOK_TOKEN, acsessToken);
		}

		string portalIdentifier = ComponentFactory.GetAComponent<IPortal>().PortalIdentifier;
		if (portalIdentifier != null)
		{
			data.Add(ServerRequestKeys.SERVER_KEY_PORTAL_KEY, portalIdentifier);
		}
		else
		{
			throw new System.Exception("ERROR - Cannot get portal identifier, cant perfotm login");
		}

		ServerRequests.SendServerRequest(command, data, (response)=>
		{
			if (completionAction != null)
			{
				completionAction();
			}
			
		}, (error) =>
		{
			Debug.LogError(error.ErrorDescription);
			if (completionAction != null)
			{
				completionAction();
			}
		}, true);
	}

	#endregion


	#region Private

	private IServerRequests ServerRequests
	{
		get
		{
			if (_serverRequestsManager == null)
			{
				_serverRequestsManager = ComponentFactory.GetAComponent<IServerRequests>() as IServerRequests;
			}
			return _serverRequestsManager;
		}
	}

	private Hashtable GetDeviceDetails()
	{
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

		return deviceDetails;
	}

	private void StoreUserToken(string token)
	{
		PlayerPrefsUtil.SetString(AUTH_TOKEN_KEY, token);
	}

	#endregion
}
