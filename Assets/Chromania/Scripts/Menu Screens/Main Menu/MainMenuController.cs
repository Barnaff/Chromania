using UnityEngine;
using System.Collections;
using System;

public class MainMenuController : BaseMenuController {


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnEnterAnimationComplete ()
	{
		Debug.Log("Enter animation complete");
	}

    public override void OnExitAnimationComplete()
    {
        throw new NotImplementedException();
    }
}
