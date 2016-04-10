using UnityEngine;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour, IBackend {

    #region Private Properties

    [SerializeField]
    private string _appID;

    [SerializeField]
    private string _apiKey;

    [SerializeField]
    private string PlayerId;

    #endregion


    #region Initialization

    void Start () {

        PlayFabSettings.TitleId = _appID;
       // PlayFabSettings.key = _apiKey;
    }

    #endregion

    #region IBackend Implementation

    public void Login(System.Action completionAction)
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = _appID,
            CreateAccount = true,
            CustomId = SystemInfo.deviceUniqueIdentifier
        };

        PlayFabClientAPI.LoginWithCustomID(request, (result) => {
            PlayerId = result.PlayFabId;
            Debug.Log("Got PlayFabID: " + PlayerId);

            if (result.NewlyCreated)
            {
                Debug.Log("(new account)");
            }
            else
            {
                Debug.Log("(existing account)");
            }

            if (completionAction != null)
            {
                completionAction();
            }
        },
        (error) => {
            Debug.LogError(error.ErrorMessage);
        });
    }

    public void FacebookConnect(string facebookAcsessToken, System.Action completionAction)
    {
        Debug.Log("linling facebook account");

        LinkFacebookAccountRequest request = new LinkFacebookAccountRequest()
        {
            AccessToken = facebookAcsessToken
        };

        PlayFabClientAPI.LinkFacebookAccount(request, (result) =>
        {
            Debug.Log("account linked with facebook");
            if (completionAction != null)
            {
                completionAction();
            }
        },
        (error) =>
        {
            Debug.LogError(error.ErrorMessage);
        });
    }

    public void PostScore(eGameMode gameMode, int score, eChromieType[] _selectedChromiez, System.Action completionAction)
    {
        Debug.Log("Sending score update");

        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest();

        request.Statistics = new System.Collections.Generic.List<StatisticUpdate>();

        request.Statistics.Add(new StatisticUpdate()
        {
            StatisticName = "score",
            Value = score
        });

        request.Statistics.Add(new StatisticUpdate()
        {
            StatisticName = "gameMode",
            Value = (int)gameMode
        });

       

        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) =>
        {
            Debug.Log("finished sending score Update");

            if (completionAction != null)
            {
                completionAction();
            }

        }, (error) =>
        {
            Debug.LogError(error.ErrorMessage);
        });
    }

    #endregion

}
