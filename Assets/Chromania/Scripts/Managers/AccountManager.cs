using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccountManager : Kobapps.Singleton<AccountManager>
{
    #region Private Properties

    [SerializeField]
    private int _accountLevel = 0;

    [SerializeField]
    private bool _tutorialEnabled = false;

    private const string TUTORIAL_ENABLED_KEY = "tutorialEnabled";

    #endregion


    #region Initialization

    void Awake()
    {
        if (PlayerPrefsUtil.HasKey(TUTORIAL_ENABLED_KEY))
        {
            _tutorialEnabled = PlayerPrefsUtil.GetBool(TUTORIAL_ENABLED_KEY);
        }
    }

    #endregion

    #region Public

    public bool TutorialEnabled
    {
        get
        {
            return _tutorialEnabled;
        }
        set
        {
            _tutorialEnabled = value;
            PlayerPrefsUtil.SetBool(TUTORIAL_ENABLED_KEY, _tutorialEnabled);
        }
    }

    public void Autologin(System.Action completionAction, System.Action failAction)
    {

        ServerRequestsManager.Instance.AuthonticateDevice((authonticationResponse) =>
        {
            if (authonticationResponse.HasErrors)
            {
                if (failAction != null)
                {
                    failAction();
                }
                Debug.LogError(authonticationResponse.Errors.ToString());
            }
            else
            {
                if (FacebookConfig.Instance.IsFacebookEnabled)
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
                else
                {
                    if (completionAction != null)
                    {
                        completionAction();
                    }
                }
               
            }

        });
        
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
