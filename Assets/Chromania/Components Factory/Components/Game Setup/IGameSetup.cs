using UnityEngine;
using System.Collections;

public interface IGameSetup
{

    int AddChromie(eChromieType chromieType);

    int RemoveChromie(eChromieType chromieType);

    bool RemoveChromieAtIndex(int index);

    bool IsSelected(eChromieType chromieType);

    eChromieType[] SelectedChromiez { get; }

    bool CanAddChromie();

    eGameMode SelectedGameMode { get; set; }

}
