using UnityEngine;
using System.Collections.Generic;
using Facebook;
using Facebook.Unity;

public class FacebookManager : FactoryComponent , IFacebookManager {


    #region Private Properties

    [SerializeField]
    private bool _isInitialized;

    [SerializeField]
    private List<string> _readPermissions = new List<string>() { "public_profile", "email", "user_friends" };

    [SerializeField]
    private List<string> _writePermissions = new List<string>() { "publish_actions" };

    #endregion


    #region FactoryComponent Implementation

    public override void InitComponentAtStart()
    {
        CallFBInit();

    }

    public override void InitComponentAtAwake()
    {
       
    }
    
    #endregion


    #region IFacebookManager

    public string AcsessToken
    {
        get
        {
            if (Facebook.Unity.AccessToken.CurrentAccessToken != null)
            {
                return Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
            }
            return "";
        }
    }

    public bool IsLoggedIn
    {
        get
        {
            return FB.IsLoggedIn;
        }
    }

    public void FacebookLogin(System.Action completionAction)
    {
        Debug.Log("starting facebook login");
        FB.LogInWithPublishPermissions(_writePermissions, (result)=>
        {

            Debug.Log("facebook Login finished token: " + result.AccessToken);
            if (completionAction != null)
            {
                completionAction();
            }
        });
    }

    public void FacebookLogout(System.Action completionAction)
    {
        
    }


    #endregion


    #region private

    private void CallFBInit()
    {
        Debug.Log("FB Init");
        FB.Init(OnInitCompleteHandler, OnHideUnityHandler);
    }

    private void CallFBLogin()
    {
        FB.LogInWithReadPermissions(_readPermissions, OnLoginCompleteHandler);
    }

    private void CallFBLoginForPublish()
    {
        // It is generally good behavior to split asking for read and publish
        // permissions rather than ask for them all at once.
        //
        // In your own game, consider postponing this call until the moment
        // you actually need it.
        FB.LogInWithPublishPermissions(_writePermissions, OnLoginCompleteHandler);
    }

    private void CallFBLogout()
    {
        FB.LogOut();
    }

    #endregion


    #region Facebook Events Handlers

    private void OnInitCompleteHandler()
    {
        _isInitialized = true;
    }

    private void OnHideUnityHandler(bool isGameShown)
    {
        
    }

    private void OnLoginCompleteHandler(ILoginResult result)
    {
        Debug.Log("FB Login");
        Debug.Log("Acsess token: " + result.AccessToken.TokenString);
      
    }

    #endregion
}