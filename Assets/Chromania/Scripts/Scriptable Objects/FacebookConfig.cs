using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FacebookConfig : Kobapps.ScriptableSingleton<FacebookConfig> {

    #region Public

    public bool EnableFacebook = true;

    public List<string> Permissions = new List<string>() { "public_profile", "email", "user_friends" };

    #endregion
}
