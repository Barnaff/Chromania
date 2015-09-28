using UnityEngine;
using System.Collections;

public class GameSetupManager : MonoBehaviour, IGameSetup {

	#region Public

	public ColorType[] _selectedColors;
	public GameModeType _selectedGameMode;

	#endregion


	#region Initialize

	void Awake()
	{
		if (_selectedColors == null || _selectedColors.Length == 0)
		{
			_selectedColors = new ColorType[4];
			_selectedColors[0] = ColorType.None;
			_selectedColors[1] = ColorType.None;
			_selectedColors[2] = ColorType.None;
			_selectedColors[3] = ColorType.None;
		}

	}

	#endregion


	#region IGameSetup Implementation

	public ColorType[] SelectedColors 
	{ 
		get
		{
			return _selectedColors;
		}
		set
		{
			_selectedColors = value;
		}
	}
	
	public GameModeType SelectedGameMode 
	{ 
		get
		{
			return _selectedGameMode;
		}
		set
		{
			_selectedGameMode = value;
		}
	}

	#endregion
}
