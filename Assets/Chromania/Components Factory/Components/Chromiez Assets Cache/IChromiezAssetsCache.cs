using UnityEngine;
using System.Collections;

public interface IChromiezAssetsCache  {


    Sprite GetChromieSprite(eChromieType colorType);

    GameObject GetChromieCharacter(eChromieType colorType);

    void LoadAll();

    void ClearCache();

}
