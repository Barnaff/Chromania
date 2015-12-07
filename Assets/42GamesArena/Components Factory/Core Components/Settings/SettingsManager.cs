using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour , ISettings{

	[SerializeField]
	private bool _musicEnabled;

	[SerializeField]
	private bool _soundFXEnabled;

	private const string MUSIC_ENABLED_KEY = "musicEnabled";
	private const string SOUND_FX_ENABLED_KEY = "soundFXEnabled";


	void Start()
	{
		if (PlayerPrefsUtil.HasKey(MUSIC_ENABLED_KEY))
		{
			_musicEnabled = PlayerPrefsUtil.GetBool(MUSIC_ENABLED_KEY);
		}
		else
		{
			_musicEnabled = true;
		}

		if (PlayerPrefsUtil.HasKey(SOUND_FX_ENABLED_KEY))
		{
			_soundFXEnabled = PlayerPrefsUtil.GetBool(SOUND_FX_ENABLED_KEY);
		}
		else 
		{
			_soundFXEnabled = true;
		}

		SoundManager.MuteMusic(!_musicEnabled);
		SoundManager.MuteSFX(!_soundFXEnabled);
	}

	#region ISettings implementation

	public bool MusicEnabled {
		get {
			return _musicEnabled;
		}
		set {
			_musicEnabled = value;
			PlayerPrefsUtil.SetBool(MUSIC_ENABLED_KEY, _musicEnabled);
			SoundManager.MuteMusic(!_musicEnabled);
		}
	}

	public bool SoundFXEnabled {
		get {
			return _soundFXEnabled;
		}
		set {
			_soundFXEnabled = value;
			PlayerPrefsUtil.SetBool(SOUND_FX_ENABLED_KEY, _soundFXEnabled);
			SoundManager.MuteSFX(!_soundFXEnabled);
		}
	}

	#endregion
}
