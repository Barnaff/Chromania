using UnityEngine;
using System.Collections;

public class GameplaySettings : Kobapps.ScriptableSingleton<GameplaySettings> {

    #region Public Properties

    public int NumberOfChromiezSlots = 4;

    public int BaseClassicNumberOfLives = 3;

    public float BaseRushGameTime = 90f;

    public float GameSpeedMultiplier = 1f;

    public int NumberOfKeepPlaying = 1;

    public float KeepPlayingTImerDuration = 10f;

    public int[] ChromiezIdsForTutorial;

    #endregion
}
