using UnityEngine;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour, IBackend {

    [SerializeField]
    private string _appID;

    [SerializeField]
    private string _apiKey;

    [SerializeField]
    private string PlayerId;

	// Use this for initialization
	void Start () {

        PlayFabSettings.TitleId = _appID;
       // PlayFabSettings.key = _apiKey;

        Login();
    }
	
	// Update is called once per frame
	void Update () {

        
    }


    void Login()
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
        },
        (error) => {
            Debug.Log("Error logging in player with custom ID:");
            Debug.Log(error.ErrorMessage);
        });
    }
}
