using System.Collections.Generic;

public delegate void FacebookStateChangedDelegate();

public interface IFacebookManager {


    void FacebookLogin(System.Action completionAction);

    void FacebookLogout(System.Action completionAction);

    bool IsLoggedIn { get; }

    string AcsessToken { get; }
}

