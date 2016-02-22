using UnityEngine;
using System.Collections;

public interface IFlow  {

    void StartGame();

    void FinishGame();

    void MainMenu();

    void DisplayMenuScreen(eMenuScreenType menuScreenType, bool animated = true);
}
