using UnityEngine;
using System.Collections;
using System;

public class BaseMenuScreen : MonoBehaviour {


    #region Private Properties

    private Action _enterAnimationCompletion;
	private Action _exitAnimationCompletion;

    private bool _needToDisplayEnterAnimation;

    [SerializeField]
    private MenuItemController[] _menuItems;

	#endregion


  

	#region Public

	/// <summary>
	/// Displaies the enter animation with completion.
	/// </summary>
	/// <param name="completion">Completion.</param>
	public void DisplayEnterAnimationWithCompletion(Action completion)
	{
		_enterAnimationCompletion = completion;
        StartCoroutine(EnterAnimationCorutine());

    }

	/// <summary>
	/// Ds the isplay exit animation with completion.
	/// </summary>
	/// <param name="completion">Completion.</param>
	public void DisplayExitAnimationWithCompletion(Action completion)
	{
		_exitAnimationCompletion = completion;
        StartCoroutine(ExitAnimationCorutine());

		
	}

	/// <summary>
	/// Screens the will be displayed.
	/// </summary>
	public virtual void ScreenWillBeDisplayed()
	{

	}

	/// <summary>
	/// Screens the will be removed.
	/// </summary>
	public virtual void ScreenWillBeRemoved()
	{

	}

	/// <summary>
	/// Begins the enter animation.
	/// </summary>
	public virtual void BeginEnterAnimation()
	{
		EnterAnimationCompleted();
	}

	/// <summary>
	/// Begins the exit animation.
	/// </summary>
	public virtual void BeginExitAnimation()
	{
		ExitAnimationFinished();
	}


	#endregion 


	#region Protected

	/// <summary>
	/// Enters the animation completed.
	/// </summary>
	protected void EnterAnimationCompleted()
	{
		if (_enterAnimationCompletion != null)
		{
			_enterAnimationCompletion();
		}
		_enterAnimationCompletion = null;
	}
	
	/// <summary>
	/// Exits the animation finished.
	/// </summary>
	protected void ExitAnimationFinished()
	{
		if (_exitAnimationCompletion != null)
		{
			_exitAnimationCompletion();
		}
		_exitAnimationCompletion = null;
	}

    #endregion

    IEnumerator EnterAnimationCorutine()
    {
        for (int i = 0; i < _menuItems.Length; i++)
        {
            _menuItems[i].gameObject.SetActive(false);
        }

        yield return null;

        for (int i=0; i< _menuItems.Length; i++)
        {
            _menuItems[i].gameObject.SetActive(true);
            _menuItems[i].MoveIn();
        }
        yield return new WaitForSeconds(0.2f);
        EnterAnimationCompleted();

    }

    IEnumerator ExitAnimationCorutine()
    {
        yield return null;
        for (int i = 0; i < _menuItems.Length; i++)
        {
            _menuItems[i].MoveOut();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        ExitAnimationFinished();
    }

}
