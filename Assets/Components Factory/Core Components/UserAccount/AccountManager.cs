using UnityEngine;
using System.Collections;
using Parse;

public class AccountManager : MonoBehaviour, IAccount {

	#region Private Properties

	private const string USER_NAME_KEY = "userName";
	private const string PASSWORD_KEY = "password";

	[SerializeField]
	private bool _UserLogedIn;

	[SerializeField]
	private bool _facebbokEnabled;

	#endregion

	// Use this for initialization
	void Start () {
	
		AutoLogin((sucsess)=>
		          {

		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region IAccount implementation

	public void AutoLogin (System.Action<bool> completionAction)
	{
		bool hasLocalUser = HasLocalUser();
		if (hasLocalUser)
		{
			Debug.Log("login with local user");
			StartCoroutine(LoginLocalUser(completionAction));
		}
		else
		{
			Debug.Log("registering new user");
			StartCoroutine(RegisterAnonymusUser(completionAction));
		}
	}

	#endregion


	#region Private

	private bool HasLocalUser()
	{
		if (PlayerPrefs.HasKey(USER_NAME_KEY) && PlayerPrefs.HasKey(PASSWORD_KEY))
		{
			return true;
		}
		return false;
	}

	private void StoreLocalUser(string userName, string password)
	{
		PlayerPrefs.SetString(USER_NAME_KEY, userName);
		PlayerPrefs.SetString(PASSWORD_KEY, password);
	}

	private void UserLogedIn()
	{
		_UserLogedIn = true;
	}

	IEnumerator RegisterAnonymusUser(System.Action <bool> completionAction)
	{
		string userName = System.Guid.NewGuid().ToString();
		string password = System.Guid.NewGuid().ToString();
		
		ParseUser user = new ParseUser();
		user.Password = password;
		user.Username = userName;

		bool finished = false;
		bool result = false;

		user.SignUpAsync().ContinueWith(t =>
		                                {
			finished = true;
			if (t.IsFaulted || t.IsCanceled)
			{
				Debug.Log("Registration failed! " + t.Exception.ToString());
				result = false;
			}
			else
			{
				// Register was successful.
				Debug.Log("Registration was successful!");
				result = true;
			}
		});

		while (!finished)
		{
			yield return null;
		}

		StoreLocalUser(userName, password);
		UserLogedIn();
		if (completionAction != null)
		{
			completionAction(true);
		}
	}

	IEnumerator LoginLocalUser(System.Action <bool> completionAction)
	{
		string userName = PlayerPrefs.GetString(USER_NAME_KEY);
		string password = PlayerPrefs.GetString(PASSWORD_KEY);

		bool finished = false;
		bool result = false;

		Debug.Log("login: " + userName + " : " + password);

		ParseUser.LogInAsync(userName, password).ContinueWith(t =>
		                                                      {
			finished = true;
			if (t.IsFaulted || t.IsCanceled)
			{
				Debug.Log("Login failed! " + t.Exception.ToString());
				result = false;
			}
			else
			{
				// Login was successful.
				Debug.Log("Login was successful");
				result = true;
			}
		});

		while (!finished)
		{
			yield return null;
		}

		UserLogedIn();

		if (completionAction != null)
		{
			completionAction(true);
		}
	}


	#endregion
}
