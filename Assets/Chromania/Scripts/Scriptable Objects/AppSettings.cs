using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppSettings : Kobapps.ScriptableSingleton<AppSettings> {

    public bool EnableDebug;

    public List<ShopItem> ShopItems;
}
