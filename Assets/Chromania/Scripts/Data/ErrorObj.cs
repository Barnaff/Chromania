using UnityEngine;
using System.Collections;

public class ErrorObj  {

	public int ErrorCode;

	public string ErrorDescription;

	public static ErrorObj CreateWithData(Hashtable data)
	{
		return null;
	}

	public static ErrorObj CreateWithError(int errorCode, string errorDescription)
	{
		ErrorObj errorObj = new ErrorObj();
		errorObj.ErrorCode = errorCode;
		errorObj.ErrorDescription = errorDescription;
		return errorObj;
	}
}
