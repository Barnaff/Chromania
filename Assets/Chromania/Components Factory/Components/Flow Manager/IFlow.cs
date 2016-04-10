using UnityEngine;
using System.Collections;

public interface IFlow  {

    void Autologin(System.Action completionAction);

    void FacebookConnect(System.Action completionAction);

    void StartGame();

    void FinishGame(GameplayTrackingData gameplayTrackingData);

    void MainMenu();

    void DisplayMenuScreen(eMenuScreenType menuScreenType, bool animated = true);
}
