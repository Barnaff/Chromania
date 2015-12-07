using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text; 


public enum RequestMethodType
{
	Post, 
	Get
}

[ExecuteInEditMode]
public class NetworkManager :MonoBehaviour , INetwork {

	#region Private Properties

	[SerializeField]
	private string _serverURL;
	
	[SerializeField]
	private bool _cacheRequests = true;
	
	[SerializeField]
	private bool _enableDebug = true;
	
	[SerializeField]
	private int _retryCount = 3;
	
	[SerializeField]
	private float _retryTimer = 1.0f;

	[SerializeField]
	private string _resultString = "Result";

	[SerializeField]
	private string _errorString = "IsError";


	private float _retryTimerCount = 0.0f;
	private List<CachedRequest> _cachedRequestsList;

	#endregion


	#region Initialization

	void Awake()
	{
		if (_cacheRequests)
		{
			_cachedRequestsList = new List<CachedRequest>();
		}
	}

	#endregion


	#region Runtime

	void Update()
	{
		if (_cacheRequests)
		{
			_retryTimerCount += Time.deltaTime;
			if (_retryTimerCount > _retryTimer)
			{
				SendCachedRequests();
				_retryTimerCount = 0;
			}
		}
	}

	#endregion

#if UNITY_EDITOR

	public bool CacheRequests
	{
		set
		{
			_cacheRequests = value;
		}
		get
		{
			return _cacheRequests;
		}
	}

	public bool EnableDebug
	{
		set
		{
			_enableDebug = value;
		}
		get
		{
			return _enableDebug;
		}
	}

	public int RetryCount
	{
		get
		{
			return _retryCount;
		}
		set
		{
			_retryCount = value;
		}
	}

	public float RetryTimer
	{
		get
		{
			return _retryTimer;
		}
		set
		{
			_retryTimer = value;
		}
	}

	public string ResultString
	{
		get
		{
			return _resultString;
		}
		set
		{
			_resultString = value;
		}
	}

	public string ErrorString
	{
		get
		{
			return _errorString;
		}
		set
		{
			_errorString = value;
		}
	}


#endif


	#region Public
	
	public event NetworkResultDelegate OnNetworkResult;

	/// <summary>
	/// Posts the server command.
	/// </summary>
	/// <param name="command">Command.</param>
	/// <param name="data">Data.</param>
	/// <param name="sucsessBlock">Sucsess block.</param>
	/// <param name="failBlock">Fail block.</param>
	public void PostServerCommand(string command, Hashtable data, System.Action <Hashtable> sucsessBlock,  System.Action <ServerError> failBlock)
	{
		PostServerCommand(command, data, RequestMethodType.Post, sucsessBlock, failBlock);
	}

	/// <summary>
	/// Posts the server command.
	/// </summary>
	/// <param name="command">Command.</param>
	/// <param name="data">Data.</param>
	/// <param name="sucsessBlock">Sucsess block.</param>
	/// <param name="failBlock">Fail block.</param>
	/// <param name="authonticate">If set to <c>true</c> authonticate.</param>
	/// <param name="methodType">Method type.</param>
	/// <param name="synchronous">If set to <c>true</c> synchronous.</param>
	public void PostServerCommand(string command, Hashtable data, RequestMethodType methodType, System.Action <Hashtable> sucsessBlock,  System.Action <ServerError> failBlock)
	{
		StartCoroutine(SendRequest(command, data, methodType, sucsessBlock, failBlock));
	}

	public string ServerURL
	{
		set
		{
			_serverURL = value;
		}
		get
		{
			return _serverURL;
		}
	}
	
	#endregion


	#region Private

	/// <summary>
	/// Sends the request.
	/// </summary>
	/// <returns>The request.</returns>
	/// <param name="command">Command.</param>
	/// <param name="data">Data.</param>
	/// <param name="sucsessBlock">Sucsess block.</param>
	/// <param name="failBlock">Fail block.</param>
	public IEnumerator SendRequest (string command, Hashtable data, RequestMethodType methodType, System.Action <Hashtable> sucsessBlock,  System.Action <ServerError> failBlock, int retryCount = 0)
	{
		string commandString = this._serverURL + command;
		if (data == null)
		{
			data = new Hashtable();
		}

		string methodTypeString = "";
		switch (methodType)
		{
		case RequestMethodType.Post:
		{
			methodTypeString = "POST";
			break;
		}
		case RequestMethodType.Get:
		{
			methodTypeString = "GET";
			break;
		}
		default:
		{
			if (_enableDebug)
			{
				Debug.LogError("<color=red> Error for Command: " + command + " : Method Type: " + methodType + " is Unsupported! </color>" );
			}
			break;
		}
		}

		HTTP.Request request = new HTTP.Request(methodTypeString,  commandString);
		
		request.headers.Add("Content-Type", "application/json");
		request.Bytes = Encoding.UTF8.GetBytes( JSON.JsonEncode( data ) );

		
		if (_enableDebug)
		{
			string requestString = NetworkDebugUtil.PrintHash(data);
			Debug.Log("<color=green> Sending: " + commandString + " : " + requestString + "</color>");
		}

		yield return request.Send();

		if(request.exception != null)
		{
			if (_enableDebug)
			{
				Debug.LogError("<color=red> Error for Command: " + command + " : " + request.exception + "</color>" );
			}

			if (request.exception.ToString().Contains("Network is unreachable") || request.exception.ToString().Contains("No such host is known"))
			{
				if (_cacheRequests)
				{
					if (_retryCount < retryCount)
					{
						CachedRequest cacheRequest = new CachedRequest(command, data, methodType, sucsessBlock, failBlock);
						cacheRequest.RetryCount = retryCount;
						_cachedRequestsList.Add(cacheRequest);
					}
					else
					{
						ServerError error = ServerError.CreateWithError(0, "ERROR - Faild sending command: " + command + " after: " + retryCount + " tries");
						failBlock(error);
						if (_enableDebug)
						{
							Debug.LogError("<color=red>" + error + "</color>" );
						}
					}
				}
				else
				{
					ServerError error = ServerError.CreateWithError(0, request.exception.ToString());
					failBlock(error);
				}
			}
		}
		else
		{
			if (_enableDebug)
			{
				Debug.Log("<color=orange>" + command + "</color>:<color=yellow>" + request.response.Text + "</color>");
			}
			
			Hashtable result = JSON.JsonDecode(request.response.Text) as Hashtable;
			
			if ( result != null  )
			{
				bool isError = result.ContainsKey(_errorString) && result[_errorString] != null;
				if (isError)
				{
					Hashtable errorData = result[_errorString] as Hashtable;
					ServerError error = ServerError.CreateWithData(errorData);
					failBlock(error);
				}
				else
				{
					Hashtable resultData = result[_resultString] as Hashtable;
					sucsessBlock( resultData );

					if (OnNetworkResult != null)
					{
						OnNetworkResult(resultData);
					}
				}
			}
			else
			{
				ServerError error = ServerError.CreateWithError(0, "Could not parse JSON response!");
				failBlock(error);

				if (_enableDebug)
				{
					Debug.LogError("<color=red> Error for Command: " + command + " : Could not parse JSON response! </color>" );
				}
			}

		}
	} 


	private void SendCachedRequests()
	{
		if (_cachedRequestsList.Count > 0)
		{
			List<CachedRequest> requestsToSend = new List<CachedRequest>();
			foreach (CachedRequest cachedRequest in _cachedRequestsList)
			{
				requestsToSend.Add(cachedRequest);
			}
			foreach (CachedRequest cachedRequest in requestsToSend)
			{
				_cachedRequestsList.Remove(cachedRequest);
				StartCoroutine(SendRequest(cachedRequest.Command, cachedRequest.Data, cachedRequest.MethodType, cachedRequest.SucsessBlock, cachedRequest.FailBlock, ++cachedRequest.RetryCount));
			}
		}
	}


#endregion
}


public static class NetworkDebugUtil
{
	public static string PrintHash(Hashtable hash)
	{
		string output = "";
		foreach (DictionaryEntry o in hash)
		{
			if (o.Value is Hashtable)
			{
				output += o.Key + ":" + NetworkDebugUtil.PrintHash(o.Value as Hashtable) + ", ";
			}
			else
			{
				output += o.Key + ":" + o.Value +", ";
			}
		}
		return output;
	}
}


public class CachedRequest
{
	public string Command;
	public Hashtable Data;
	public RequestMethodType MethodType;
	public System.Action <Hashtable> SucsessBlock;
	public System.Action <ServerError> FailBlock;
	public int RetryCount = 0;

	public CachedRequest(string command, Hashtable data, RequestMethodType methodType, System.Action <Hashtable> sucsessBlock,  System.Action <ServerError> failBlock)
	{
		Command = command;
		Data = data;
		SucsessBlock = sucsessBlock;
		FailBlock = failBlock;
		MethodType = methodType;
	}
}


public class ServerError
{
	public string ErrorDescription;
	public int ErrorCode;
	public string ErrorMessage;

	public const string ERROR_CODE_KEY = "errorCode";
	public const string ERROR_DESCRIPTION_KEY = "errorDesc";
	public const string ERROR_MESSAGE_KEY = "errorMessage";

	public ServerError(int errorCode, string errorDescription, string errorMessage)
	{
		ErrorCode = errorCode; 
		ErrorDescription = errorDescription;
		ErrorMessage = errorMessage;
	}

	public static ServerError CreateWithData(IDictionary data)
	{
		ServerError error = new ServerError(int.Parse(data[ERROR_CODE_KEY].ToString()), data[ERROR_DESCRIPTION_KEY].ToString(), data[ERROR_MESSAGE_KEY].ToString());
		return error;
	}

	public static ServerError CreateWithError(int errorCode, string errorDescription, string errorMessage = "")
	{
		ServerError error = new ServerError(errorCode, errorDescription, errorMessage);
		return error;
	}
}
