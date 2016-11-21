using UnityEngine;
using System.Collections;
using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System.Collections.Generic;

public class ServerRequestsManager : Kobapps.Singleton<ServerRequestsManager> {

    #region Public

    public void Init(System.Action completionAction, System.Action failAction)
    {
        StartCoroutine(InitNetworkCorutine(completionAction));
    }

    public bool NetworkAvalable
    {
        get
        {
            return GS.Available;
        }
    }

    public string SDKVersion
    {
        get
        {
            return GS.Version.ToString();
        }
    }

    public bool Authenticated
    {
        get
        {
           return GS.Authenticated;
        }
    }


    public void ConnectFacebook(string facebookToken, System.Action <AuthenticationResponse> completionAction, bool durable = false)
    {
        new FacebookConnectRequest().SetDurable(durable).SetAccessToken(facebookToken).Send((response) =>
        {
            if (completionAction != null)
            {
                completionAction(response);
            }
        });
    }

    public void AuthonticateDevice(System.Action <AuthenticationResponse> completionAction, bool durable = false)
    {
        new DeviceAuthenticationRequest().SetDurable(durable).Send((response) => 
        {
            if (completionAction != null)
            {
                completionAction(response);
            }
        });
    }

    public void AccoutDetails(System.Action <AccountDetailsResponse> completionAction, bool durable = false)
    {
        new AccountDetailsRequest().SetDurable(durable).Send((response) => 
        {
            if (completionAction != null)
            {
                completionAction(response);
            }
        });
    }

    public void Reset()
    {
        GS.Reset();
    }

    public void Disconnet()
    {
        GS.Disconnect();
    }

    public void PostLeaderboardEntry(GameplayTrackingData gameplayTrackingData, System.Action completionAction)
    {
        new LogEventRequest_POST_CS_SCORE().Set_SCORE(gameplayTrackingData.Score)
                .Set_COLOR_1((int)gameplayTrackingData.SelectedColors[0])
                .Set_COLOR_2((int)gameplayTrackingData.SelectedColors[1])
                .Set_COLOR_3((int)gameplayTrackingData.SelectedColors[2])
                .Set_COLOR_4((int)gameplayTrackingData.SelectedColors[3])
                .Send((response) =>
                {
                    if (completionAction != null)
                    {
                        completionAction();
                    }
                });
    }

    public void GetLeaderboard(eGameplayMode gameplayMode, System.Action <List<object>> completionAction)
    {

    }
    

    #endregion


    #region Private

    private IEnumerator InitNetworkCorutine(System.Action completionAction)
    {
        this.gameObject.AddComponent<GameSparksUnity>();

        yield return null;

        while (!NetworkAvalable)
        {
            yield return null;
        }
       
        if (completionAction != null)
        {
            completionAction();
        }
    }

    #endregion
}
