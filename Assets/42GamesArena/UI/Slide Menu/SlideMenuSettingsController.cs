using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlideMenuSettingsController : MonoBehaviour {

	[SerializeField]
	private Text _versionLabel;

	[SerializeField]
	private Toggle _soundEffectToggle;

	[SerializeField]
	private Toggle _musicToggle;

	// Use this for initialization
	void Start () {
	
		string versionString = "";
		if (PlayerPrefsUtil.HasKey("currentVersion"))
		{
			versionString = "Version: " + PlayerPrefsUtil.GetString("currentVersion");
		}
		else
		{
			versionString = "Version is invalid!";
		}
		_versionLabel.text = versionString;

		ISettings settingManager = ComponentFactory.GetAComponent<ISettings>();
		if (settingManager != null)
		{
			_musicToggle.isOn = settingManager.MusicEnabled;
			_soundEffectToggle.isOn = settingManager.SoundFXEnabled;
		}
	}
	

	public void ToggleSoundEffects(bool value)
	{
		ISettings settingManager = ComponentFactory.GetAComponent<ISettings>();
		if (settingManager != null)
		{
			settingManager.SoundFXEnabled = _soundEffectToggle.isOn;
		}
	}

	public void ToggleBackgroundMusic(bool value)
	{
		ISettings settingManager = ComponentFactory.GetAComponent<ISettings>();
		if (settingManager != null)
		{
			settingManager.MusicEnabled = _musicToggle.isOn;
		}
	}
}
