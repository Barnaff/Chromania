using UnityEngine;
using System.Collections;

public interface IFlow  {

    void StartGame();

    void FinishGame(GameplayTrackingData gameplayTrackingData);

    void MainMenu();

    void DisplayMenuScreen(eMenuScreenType menuScreenType, bool animated = true);
}
