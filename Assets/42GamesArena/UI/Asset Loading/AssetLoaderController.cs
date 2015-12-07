using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AssetLoaderController : MonoBehaviour {

	#region Public Properties

	public Image LoaderBar;

	public Text ProgressLabel;

	#endregion


	// Use this for initialization
	void Start () {
	
		AssetManager.instance.OnProgressUpdate += UpdateProgress;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	private void UpdateProgress(float progressValue)
	{
		if (LoaderBar != null)
		{
			LoaderBar.rectTransform.localScale = new Vector3(progressValue, 1, 1);
		}

		if (ProgressLabel != null)
		{
			ProgressLabel.text = (progressValue * 100.0f).ToString("F2") + "%";
		}
	}

	private void FinishedLoadingAsset()
	{

	}


}
