using UnityEngine;
using System.Collections;

public interface IBackend
{
    void Login(System.Action completionAction);

    void FacebookConnect(string facebookAcsessToken, System.Action completionAction);

    void PostScore(eGameMode gameMode, int score, eChromieType[] _selectedChromiez, System.Action completionAction);
	
}
