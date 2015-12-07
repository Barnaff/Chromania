using UnityEngine;
using System.Collections;

public class BackgroundThreadUtil : MonoBehaviour {


	public static void RunBackgroundAction(System.Action backgroundAction, System.Action completionAction)
	{
		GameObject backgrounObject = new GameObject();
		backgrounObject.name = "Background Task";
		BackgroundThreadUtil backgroundThreadUtil = backgrounObject.AddComponent<BackgroundThreadUtil>() as BackgroundThreadUtil;
		backgroundThreadUtil.RunBackgroundTask(backgroundAction, completionAction);
		DontDestroyOnLoad(backgrounObject);

	}

	private void RunBackgroundTask(System.Action backgorundAction, System.Action completionAction)
	{
		StartCoroutine(BackgroundTaskCorutine(backgorundAction, completionAction));
	}

	IEnumerator BackgroundTaskCorutine(System.Action backgorundAction, System.Action completionAction)
	{
		ThreadedAction threadedAction = new ThreadedAction(backgorundAction);
		yield return StartCoroutine(threadedAction.WaitForComplete());
		yield return null;
		if (completionAction != null)
		{
			completionAction();
		}
		Destroy(this.gameObject);
	}

}

public class ThreadedAction
{
	private bool _isDone = false;

	public ThreadedAction(System.Action action)
	{
		var thread = new System.Threading.Thread(() => {
			if(action != null)
			{
				action();
			}
			_isDone = true;
		});
		thread.Start();
	}
	
	public IEnumerator WaitForComplete()
	{
		while (!_isDone)
		{
			yield return null;
		}
		yield return null;
	}

}
