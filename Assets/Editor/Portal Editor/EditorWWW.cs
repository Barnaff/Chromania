using UnityEngine;
using UnityEditor;

public class EditorWWW  {
	
	private WWW _www;
	private System.Action <WWW> _completionAction;

	public EditorWWW(WWW www, System.Action <WWW> completionAction)
	{
		_www = www;
		_completionAction = completionAction;
		EditorApplication.update += Update;

	}

	private void Update()
	{
		if (_www.isDone)
		{
			EditorApplication.update -= Update;
			LoadCompleted();
		}
	}


	private void LoadCompleted()
	{
		if (_completionAction != null)
		{
			_completionAction(_www);
		}
	}


}
