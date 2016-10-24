using UnityEngine;
using System.Collections;

public class AccountManager : Kobapps.Singleton<AccountManager>
{
    #region Private Properties


    #endregion


    #region Public

    public void Autologin(System.Action completionAction, System.Action failAction)
    {
        if (completionAction != null)
        {
            completionAction();
        }
        /*
        ServerRequestsManager.Instance.AuthonticateDevice((authonticationResponse) =>
        {
            if (authonticationResponse.HasErrors)
            {
                if (failAction != null)
                {
                    failAction();
                }
            }
            else
            {
                FacebookManager.Instance.Connect(() =>
                {
                    if (!string.IsNullOrEmpty(FacebookManager.Instance.FacebookAccessToekn))
                    {
                        ServerRequestsManager.Instance.ConnectFacebook(FacebookManager.Instance.FacebookAccessToekn, (facebookConnectResponse) =>
                        {
                            if (facebookConnectResponse.HasErrors)
                            {
                                Debug.Log("Sucsess facebook connect");

                                if (completionAction != null)
                                {
                                    completionAction();
                                }
                            }
                            else
                            {
                                if (failAction != null)
                                {
                                    failAction();
                                }
                            }

                        });
                    }

                }, () =>
                {
                    ServerRequestsManager.Instance.ConnectFacebook(FacebookManager.Instance.FacebookAccessToekn, (facebookConnectResponse) =>
                    {
                        if (facebookConnectResponse.HasErrors)
                        {
                            Debug.Log("Sucsess facebook connect");

                            if (completionAction != null)
                            {
                                completionAction();
                            }
                        }
                        else
                        {
                            if (failAction != null)
                            {
                                failAction();
                            }
                        }
                    });
                });
            }

        });
        */
    }


    public void Logout()
    {

    }

    public bool HasLocalUser()
    {
        return false;
    }

    #endregion
}
