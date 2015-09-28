using UnityEngine;
using System.Collections;
using System;

public class BaseMenuScreen : MonoBehaviour {

	#region Private Properties

	private Action _enterAnimationCompletion;
	private Action _exitAnimationCompletion;

    private bool _needToDisplayEnterAnimation;

	#endregion


    void LateUpdate()
    {
        if (_needToDisplayEnterAnimation)
        {
            DisplayEnterAnimationWithCompletion(_enterAnimationCompletion);
        }
    }

	#region Public

	/// <summary>
	/// Displaies the enter animation with completion.
	/// </summary>
	/// <param name="completion">Completion.</param>
	public void DisplayEnterAnimationWithCompletion(Action completion)
	{
		_enterAnimationCompletion = completion;
        if (!_needToDisplayEnterAnimation)
        {
            _needToDisplayEnterAnimation = true;
            return;
        }
        _needToDisplayEnterAnimation = false;
        MenuItemController[] menuItems = GameObject.FindObjectsOfType(typeof(MenuItemController)) as MenuItemController[];
		if (menuItems != null)
		{
			for (int i=0; i < menuItems.Length; i++)
			{
				MenuItemController itemController = menuItems[i];
				if (i == menuItems.Length - 1)
				{
					itemController.MoveIn(0.2f, ()=>
					{
						EnterAnimationCompleted();
					});
				}
				else
				{
					itemController.MoveIn();

				}
			}
		}
    }

	/// <summary>
	/// Ds the isplay exit animation with completion.
	/// </summary>
	/// <param name="completion">Completion.</param>
	public void DisplayExitAnimationWithCompletion(Action completion)
	{
		_exitAnimationCompletion = completion;

		MenuItemController[] menuItems = GameObject.FindObjectsOfType(typeof(MenuItemController)) as MenuItemController[];
		if (menuItems != null)
		{
			for (int i=0; i < menuItems.Length; i++)
			{
				MenuItemController itemController = menuItems[i];
				if (i == menuItems.Length - 1)
				{
					itemController.MoveOut(0.2f, ()=>
					                      {
						ExitAnimationFinished();
					});
					
				}
				else
				{
					itemController.MoveOut();
				}
			}
		}
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

}
