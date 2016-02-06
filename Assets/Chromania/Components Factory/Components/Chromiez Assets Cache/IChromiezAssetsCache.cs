using UnityEngine;
using System.Collections;

public interface IChromiezAssetsCache  {

    GameObject GetGameplayChromie(eChromieType colorType);

    Sprite GetChromieSprite(eChromieType colorType);

    void LoadAll();

    void ClearCache();

}
