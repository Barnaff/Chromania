using UnityEngine;
using System.Collections;

public class GameOverScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region User Interaction

	public void HomeButtonAction()
	{
		Application.LoadLevel("Lobby Scene");
	}

	public void PlayAgainButtonAction()
	{

	}

	#endregion
}
