using UnityEngine;
using System;

public class GameUtils
{
	public static DelayedCall StartDelayedCall(float time, string identifier, Action callBack)
	{
		GameObject delayedCallObject = new GameObject();
		delayedCallObject.name = identifier;
		DelayedCall delayedCall = delayedCallObject.AddComponent<DelayedCall>();
		delayedCall.StartDelayedCall(time, callBack);
		return delayedCall;
	}
}

public class DelayedCall : MonoBehaviour
{
	private Action _callBack;

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void StartDelayedCall(float time, Action callBack)
	{
		_callBack = callBack;
		Invoke("InvokeDelyedCall", time);
	}

	private void InvokeDelyedCall()
	{
		if (_callBack != null)
		{
			_callBack();
		}
		_callBack = null;
		Destroy(this.gameObject);
	}
}
