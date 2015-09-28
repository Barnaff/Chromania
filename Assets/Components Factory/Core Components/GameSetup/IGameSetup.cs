using UnityEngine;
using System.Collections;

public interface IGameSetup  {

	ColorType[] SelectedColors { get; set; }

	GameModeType SelectedGameMode { get; set; }


}
