using UnityEngine;
using System.Collections.Generic;


public class FacebookManager : MonoBehaviour , IFacebookManager {

	#region Public Properties

	[SerializeField]
	private bool _facebookEnabled;

	/// <summary>
	/// The facebook identifier.
	/// </summary>
	public string FacebookId;

	/// <summary>
	/// The facebook token.
	/// </summary>
	public string FacebookToken;

	/// <summary>
	/// The permissions list.
	/// </summary>
	[SerializeField]
	private string[] _permissionsList;

	#endregion


	#region Private Properties

	private bool _isFacebookInit;
	private System.Action  _initCompletionBlock = null;
	private System.Action <bool> _loginCompletionBlock = null;
	private System.Action <List<IFacebookProfile>> _getFriendsCompletionBlock = null;
	private System.Action <IFacebookProfile> _getUserFacebookProfile = null;
	private System.Action <bool> _inviteFriendCompletionBlock = null;
	private List<IFacebookProfile> _facebookFriends;

	#endregion


	#region Initialization

	void Awake()
	{
		_initCompletionBlock = null;
		_loginCompletionBlock = null;
		_getFriendsCompletionBlock = null;
		_inviteFriendCompletionBlock = null;
	}

	#endregion


	#region Private
	 
	private void CallFBInit()
	{
		if (!_isFacebookInit)
		{
			FB.Init(OnInitComplete, OnHideUnity);
		}
		else
		{
			Debug.LogError("ERROR - Trying to init Facebook when it was already beed initialized!");
			_initCompletionBlock();
		}
	}

	private void OnInitComplete()
	{
		Debug.Log("<color=cyan>FB.Init completed: Is user logged in? " + FB.IsLoggedIn + "</color>");
		_isFacebookInit = true;

		if (_initCompletionBlock != null)
		{
			_initCompletionBlock();
			_initCompletionBlock = null;
		}
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("<color=cyan>Is game showing? " + isGameShown + "</color>");
	}


	private void CallFBLogin()
	{
		if (FB.IsLoggedIn)
		{
			Debug.LogError("ERROR - Trying to login after user is already logged in");
			LoginCallback(null);
		}
		else
		{
			FB.Login(GetPersmissions(), LoginCallback);
		}
	}

	#endregion


	#region Public 

	public bool FacebookEnabled 
	{
		get
		{
			return _facebookEnabled;
		}
	}
	 
	public void InitFacebookWithCompletion(System.Action completionBlock)
	{
		_initCompletionBlock = completionBlock;
		CallFBInit();
	}

	public void PerformFacebookLogin(System.Action <bool> loginCompletionBlock)
	{
		if (_loginCompletionBlock != null)
		{
			Debug.LogError("ERROR - There is already a call pending for Login , aborting the call");
		}
		else
		{
			_loginCompletionBlock = loginCompletionBlock;
			if (_isFacebookInit)
			{
				if (FB.IsLoggedIn)
				{
					_loginCompletionBlock(true);
					_loginCompletionBlock = null;
				}
				else
				{
					FB.Login(GetPersmissions(), LoginCallback);  
				} 
			}
		}
	}

	public void GetMyFacebookFriends(System.Action <List<IFacebookProfile>> completionBlock)
	{
		if (_facebookFriends != null)
		{
			completionBlock(_facebookFriends);
			return;
		}

		if (_getFriendsCompletionBlock != null)
		{
			Debug.LogError("ERROR - There is already a call pending for getting friends list, aborting the call");
		}
		else
		{
			if (FB.IsLoggedIn)
			{
				_getFriendsCompletionBlock = completionBlock;
				FB.API("/me/friends", Facebook.HttpMethod.GET , GetFacebookFriendsCallback);
			}
			else
			{
				Debug.LogError("ERROR - Not Logged to facebook!");
			}
		}
	}

	public void GetFacebookUserDetails(System.Action <IFacebookProfile> completionBlock, System.Action <ErrorObj> failBlock)
	{
			FB.API("me?fields=name,id,first_name,last_name,gender,email,birthday,age_range,link,locale,location,timezone", Facebook.HttpMethod.GET , GetUserDetailsCallback);
			_getUserFacebookProfile = completionBlock;
	}

	public void InviteFriend(IFacebookProfile friendProfile, System.Action <bool> sucsessBlock, System.Action <ErrorObj> failBlock)
	{
		if (sucsessBlock != null)
		{
			_inviteFriendCompletionBlock = sucsessBlock;
		}
		string[] facebookFriendsIds = new string[] { friendProfile.FacebookId };
		FB.AppRequest("Come play with me!", facebookFriendsIds, null, null, default(int?), "", "", InviteFriendCallBack);
	}
	
	public bool IsLoggedIn()
	{
		if (FB.IsInitialized)
		{
			return FB.IsLoggedIn;
		}
		return false;
	}

	public void GetProfileForId(string facebookProfileId, System.Action <IFacebookProfile> completionAction)
	{
		GetMyFacebookFriends((facebookFriendsList)=>
		                     {
			foreach (IFacebookProfile facebookProfile in facebookFriendsList)
			{
				if (facebookProfile.FacebookId == facebookProfileId)
				{
					completionAction(facebookProfile);
				}
			}
			completionAction(null);
		});
	}

	public void ShareOnWall(string title, string caption, string url, System.Action <bool> completionAction)
	{
		FB.Feed(FB.UserId, url, "", "", "", "", "", "", "", "", null, (result)=>
		        {
			bool sucsess = false;
			Debug.Log("<color=cyan> Share completed: " + result.Text + "</color>");

			Dictionary<string, object> resultData = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string, object>;

			if (resultData != null)
			{
				if (resultData.ContainsKey("id"))
				{
					sucsess = true;
				}

				if (resultData.ContainsKey("cancelled"))
				{
					if ((bool)resultData["cancelled"] == true)
					{
						sucsess = false;
					}
				}
			}

			if (result.Error != null)
			{
				sucsess = false;
				Debug.Log("<color=cyan> ERROR Sharing: " + result.Error + "</color>");
			}

			if (sucsess)
			{
				Debug.Log("<color=cyan> Share sucseeced! </color>");
			}

			completionAction(sucsess);
		});
	}
	
	#endregion

	#region Facebook Callbacks

	private void InviteFriendCallBack(FBResult result)
	{
		if (result != null)
		{
			Debug.Log(result.Text);
		}
		if (result.Error == null)
		{
			if (_inviteFriendCompletionBlock != null)
			{
				_inviteFriendCompletionBlock(true);
			}
		}
		else
		{
			if (_inviteFriendCompletionBlock != null)
			{
				_inviteFriendCompletionBlock(false);
			}
		}

	}

	private void LoginCallback(FBResult result)
	{
		Debug.Log("<color=cyan>LoginCallback: " + result.Text + "</color>");
		if (_loginCompletionBlock != null)
		{
			_loginCompletionBlock(FB.IsLoggedIn);
			_loginCompletionBlock = null;

			FacebookToken = FB.AccessToken;
			FacebookId = FB.UserId;

			if (result.Error != null)
			{
				
				Debug.LogError(result.Error);
			}
		}
	}
	
	private void CallFBLogout()
	{
		FB.Logout();
	}
	
	private void GetFacebookFriendsCallback(FBResult result)
	{
		if (_getFriendsCompletionBlock != null)
		{
			if (result != null)
			{
				Dictionary<string, object> response = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string, object>;
				if (response.ContainsKey("data"))
				{
					List<object> friendsList = response["data"] as List<object>;
					_facebookFriends = new List<IFacebookProfile>();
					foreach (Dictionary<string, object> friendObject in friendsList)
					{
						FacebookProfile profile = new FacebookProfile();
						profile.FacebookId = friendObject["id"].ToString();
						profile.UserName = friendObject["name"].ToString();
						_facebookFriends.Add(profile);
					}
				}
			}
			_getFriendsCompletionBlock(_facebookFriends);
			_getFriendsCompletionBlock = null;
		}
	}

	private string GetPersmissions()
	{
		string permissionString = "";
		foreach (string permission in _permissionsList)
		{
			permissionString += permission + ",";
		}
		return permissionString;
	}

	private void GetUserDetailsCallback(FBResult result)
	{
		if (_getUserFacebookProfile != null)
		{
			Debug.Log("<color=cyan>GetUserDetailsCallback: " + result.Text + "</color>");
			if (result.Error == null)
			{
				FacebookProfile userProfile = FacebookProfile.CreateWithResult(result);
				_getUserFacebookProfile(userProfile);
			}
			else
			{
				Debug.LogError(result.Error.ToString());
				_getUserFacebookProfile(null);
			}
		}
	}

	#endregion	
}