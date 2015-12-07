using UnityEngine;
using System.Collections;

public class LoadingIndicatorPopupController : PopupBaseController {

	[SerializeField]
	private GameObject _spinner;
	

	// Update is called once per frame
	void Update () {
	
		if (_spinner != null)
		{
			_spinner.transform.Rotate(Vector3.forward * Time.deltaTime * 50.0f);
		}
	}

	public override void BackButtonPressed ()
	{
		// dont close this popup when back button is pressed.
	}
}
