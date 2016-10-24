using UnityEngine;
using System.Collections;

public class NetworkRequestTester : MonoBehaviour {

    #region Private Properties

    private string _facebookToken = "";

    #endregion

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


#if UNITY_EDITOR

    void OnGUI()
    {
        /*
        GUILayout.BeginVertical("Box");
        {

            GUILayout.Label("Server " + (ServerRequestsManager.Instance.NetworkAvalable ? "AVALBLE" : "NOT AVALABLE"));
            GUILayout.Label("Version " + ServerRequestsManager.Instance.SDKVersion);

            GUILayout.Label((ServerRequestsManager.Instance.Authenticated ? "AUTHENTICATED" : "NOT AUTHENTICATED"));

            if (ServerRequestsManager.Instance.NetworkAvalable)
            {
                if (GUILayout.Button("Disconnect"))
                {
                    ServerRequestsManager.Instance.Disconnet();
                }

                if (GUILayout.Button("Reset"))
                {
                    ServerRequestsManager.Instance.Reset();
                }
            }
            else
            {
                if (GUILayout.Button("Connect"))
                {
                    ServerRequestsManager.Instance.AuthonticateDevice((response) =>
                    {
                        Debug.Log("Connectd");
                    });
                }
            }


            GUILayout.BeginVertical("Box");
            {
                GUILayout.Label("Account");

                if (GUILayout.Button("Authonticate Device"))
                {
                    ServerRequestsManager.Instance.AuthonticateDevice((response) =>
                    {

                    }); 
                }


                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Facebook Token:");
                    _facebookToken = GUILayout.TextField(_facebookToken, GUILayout.Width(200));

                    if (GUILayout.Button("Connect FB"))
                    {
                        ServerRequestsManager.Instance.ConnectFacebook(_facebookToken, (resonse) =>
                        {

                        });  
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();

            


        }
        GUILayout.EndVertical();
        */
    }


#endif
}
