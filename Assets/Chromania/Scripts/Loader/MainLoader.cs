using UnityEngine;
using System.Collections;

public class MainLoader : MonoBehaviour {

    #region Privte Properties

    [SerializeField]
    private string _version;

    #endregion


    #region Initialize

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(LoadGameCorutine());
	}

    #endregion


    #region Private

    IEnumerator LoadGameCorutine()
    {
        Debug.Log("start loading game");
        
        yield return null;
    }

    #endregion
}
