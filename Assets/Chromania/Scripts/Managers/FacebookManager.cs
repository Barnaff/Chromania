using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;

public class FacebookManager : Kobapps.Singleton<FacebookManager> {

    #region Private properties

    [SerializeField]
    private string _accessToekn = null;

    #endregion

    #region Public

    public void Init(System.Action completionAction)
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {

                if (completionAction != null)
                {
                    completionAction();
                }
            }, (isGameShown) =>
            {

            });
        }
    }

    public void Connect(System.Action completionAction, System.Action failAction)
    {
        PerformFacebookAction(() =>
        {
            if (FB.IsLoggedIn)
            {
                if (completionAction != null)
                {
                    completionAction();
                }
            }
            else
            {
                FB.LogInWithReadPermissions(FacebookConfig.Instance.Permissions, (response) =>
                {
                    if (response.Error != null)
                    {
                        Debug.LogError("FB ERROR: " + response.Error);

                        if (failAction != null)
                        {
                            failAction();
                        }
                    }
                    else
                    {
                        if (AccessToken.CurrentAccessToken != null)
                        {
                            _accessToekn = AccessToken.CurrentAccessToken.TokenString;
                            Debug.Log("token: " + _accessToekn);
                        }
                        if (completionAction != null)
                        {
                            completionAction();
                        }
                    }
                });
            }
           
        });
        
    }

    public string FacebookAccessToekn
    {
        get
        {
            return _accessToekn;
        }
    }

    #endregion


    #region Private

    private void PerformFacebookAction(System.Action action)
    {
        
        if (FB.IsInitialized)
        {
            if (action != null)
            {
                action();
            }
        }
        else
        {
            Init(() =>
            {
                if (action != null)
                {
                    action();
                }
            });
        }
        
    }

    #endregion

}
