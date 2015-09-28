using UnityEngine;
using System.Collections;

public class ChromiePowerupController : MonoBehaviour {

	#region Public Properties

	public GameObject PowerupEffect;

	public ChromieDataItem ChromieData;

    public bool PowerupEnabled;

	#endregion


	#region Initialize

	void OnEnable () {
		if (PowerupEffect != null)
		{
			PowerupEffect.SetActive(false);
		}
        PowerupEnabled = false;
    }
	
	#endregion


	#region Public

	public void EnablePowerup()
	{
		PowerupEffect.SetActive(true);
        PowerupEnabled = true;
    }

	public void ActivatePowerup()
	{

	}

	#endregion
}
